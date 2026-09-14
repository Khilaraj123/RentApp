using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentApp.Application.Common.Exceptions;
using RentApp.Application.Common.Pagination.Offset;
using RentApp.Application.DTOs.Listings;
using RentApp.Application.Interfaces.External;
using RentApp.Application.Interfaces.Helpers;
using RentApp.Application.Interfaces.Listings;
using RentApp.Domain.Common.Pagination;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;
using RentApp.Domain.Repositories;
using RentApp.Domain.ValueObjects;

namespace RentApp.Application.Services.Listings;

public class ListingService : IListingService
{
    private readonly IListingRepository _listingRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISlugGenerator _slugGenerator;
    private readonly ILogger<ListingService> _logger;
    private readonly IFileStorage? _fileStorage;

    public ListingService(
        IListingRepository listingRepository,
        IUnitOfWork unitOfWork,
        ISlugGenerator slugGenerator,
        ILogger<ListingService> logger,
        IFileStorage? fileStorage = null)
    {
        _listingRepository = listingRepository;
        _unitOfWork = unitOfWork;
        _slugGenerator = slugGenerator;
        _logger = logger;
        _fileStorage = fileStorage;
    }

    #region Queries

    public async Task<ListingDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        return listing == null ? null : MapToDetailsDto(listing);
    }

    public async Task<ListingDetailsDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetBySlugWithDetailsAsync(slug, cancellationToken);
        return listing == null ? null : MapToDetailsDto(listing);
    }

    public async Task<PagedResult<ListingCardDto>> GetPagedListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default)
    {
        var query = BuildFilteredQuery(filter);
        var pagedListings = await query.ToPagedResultAsync(
            new PaginationRequest { Page = filter.Page, PageSize = filter.PageSize },
            cancellationToken);

        return pagedListings.Map(MapToCardDto);
    }

    public async Task<PagedResult<ListingCardDto>> SearchListingsAsync(ListingSearchDto search, CancellationToken cancellationToken = default)
    {
        var query = BuildFilteredQuery(search);

        if (search.StartDate.HasValue && search.EndDate.HasValue)
        {
            var start = search.StartDate.Value;
            var end = search.EndDate.Value;
            var qty = search.QuantityNeeded ?? 1;

            // Filter out listings where quantity is insufficient or unavailable during period
            query = query.Where(l => l.Quantity >= qty &&
                !l.AvailabilityRules.Any(r => !r.IsAvailable && r.Period.StartDate < end && start < r.Period.EndDate));
        }

        var pagedListings = await query.ToPagedResultAsync(
            new PaginationRequest { Page = search.Page, PageSize = search.PageSize },
            cancellationToken);

        return pagedListings.Map(MapToCardDto);
    }

    public async Task<PagedResult<ListingCardDto>> GetOwnerListingsAsync(
        Guid ownerId,
        PaginationRequest request,
        ListingStatus? status = null,
        CancellationToken cancellationToken = default)
    {
        var query = _listingRepository.QueryableNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Category)
            .Include(l => l.Owner)
            .Include(l => l.PricingRules)
            .Where(l => l.OwnerId == ownerId);

        if (status.HasValue)
        {
            query = query.Where(l => l.Status == status.Value);
        }

        query = query.OrderByDescending(l => l.CreatedAt);

        var paged = await query.ToPagedResultAsync(request, cancellationToken);
        return paged.Map(MapToCardDto);
    }

    public async Task<List<ListingCardDto>> GetFeaturedListingsAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        var listings = await _listingRepository.QueryableNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Category)
            .Include(l => l.Owner)
            .Include(l => l.PricingRules)
            .Where(l => l.Status == ListingStatus.Active)
            .OrderByDescending(l => l.AverageRating)
            .ThenByDescending(l => l.BookingCount)
            .Take(count)
            .ToListAsync(cancellationToken);

        return listings.Select(MapToCardDto).ToList();
    }

    public async Task<List<ListingCardDto>> GetRecentListingsAsync(int count = 6, CancellationToken cancellationToken = default)
    {
        var listings = await _listingRepository.QueryableNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Category)
            .Include(l => l.Owner)
            .Include(l => l.PricingRules)
            .Where(l => l.Status == ListingStatus.Active)
            .OrderByDescending(l => l.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);

        return listings.Select(MapToCardDto).ToList();
    }

    public async Task<List<ListingCardDto>> GetSimilarListingsAsync(Guid listingId, int count = 4, CancellationToken cancellationToken = default)
    {
        var target = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (target == null)
            return new List<ListingCardDto>();

        var similar = await _listingRepository.QueryableNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Category)
            .Include(l => l.Owner)
            .Include(l => l.PricingRules)
            .Where(l => l.Id != listingId && l.Status == ListingStatus.Active &&
                        (l.CategoryId == target.CategoryId || l.Condition == target.Condition))
            .OrderByDescending(l => l.AverageRating)
            .Take(count)
            .ToListAsync(cancellationToken);

        return similar.Select(MapToCardDto).ToList();
    }

    #endregion

    #region CRUD

    public async Task<ListingDetailsDto> CreateAsync(CreateListingDto dto, Guid ownerId, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);
        if (ownerId == Guid.Empty) throw new ArgumentException("Owner ID cannot be empty.", nameof(ownerId));

        var slug = await GenerateUniqueSlugAsync(dto.Title, null, cancellationToken);

        var listing = new Listing(
            ownerId,
            dto.CategoryId,
            dto.Title,
            slug,
            dto.Description,
            dto.Condition,
            dto.Quantity,
            dto.DeliveryOption);

        // Instant booking
        if (!dto.RequiresOwnerApproval)
        {
            listing.EnableInstantBooking();
        }

        // Location
        if (!string.IsNullOrWhiteSpace(dto.Street) &&
            !string.IsNullOrWhiteSpace(dto.City) &&
            !string.IsNullOrWhiteSpace(dto.State) &&
            !string.IsNullOrWhiteSpace(dto.ZipCode) &&
            !string.IsNullOrWhiteSpace(dto.Country))
        {
            var address = new Address(dto.Street, dto.City, dto.State, dto.ZipCode, dto.Country);
            GeoLocation? geo = null;
            if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                geo = new GeoLocation(dto.Latitude.Value, dto.Longitude.Value);
            }
            listing.UpdateLocation(address, geo);
        }

        // Policy
        if (dto.Policy != null)
        {
            var cancelDesc = string.IsNullOrWhiteSpace(dto.Policy.CancellationPolicyDescription)
                ? $"{dto.Policy.CancellationPolicyName} policy"
                : dto.Policy.CancellationPolicyDescription;

            var cancellationPolicy = new CancellationPolicy(dto.Policy.CancellationPolicyName, cancelDesc);
            Money? securityDeposit = dto.Policy.SecurityDepositAmount.HasValue
                ? new Money(dto.Policy.SecurityDepositAmount.Value, dto.Policy.SecurityDepositCurrency ?? "USD")
                : null;
            Money? lateFee = dto.Policy.LateFeeAmount.HasValue
                ? new Money(dto.Policy.LateFeeAmount.Value, dto.Policy.LateFeeCurrency ?? "USD")
                : null;

            var policy = new ListingPolicy(
                listing.Id,
                cancellationPolicy,
                securityDeposit,
                dto.Policy.DamagePolicy,
                lateFee,
                dto.Policy.ReturnInstructions);

            listing.SetPolicy(policy);
        }

        // Initial Pricing Rules
        if (dto.PricingRules != null)
        {
            foreach (var prDto in dto.PricingRules)
            {
                var priceMoney = new Money(prDto.Price, prDto.Currency);
                var rule = new PricingRule(listing.Id, prDto.Unit, priceMoney, prDto.MinDuration, prDto.MaxDuration);
                listing.AddPricingRule(rule);
            }
        }

        await _listingRepository.AddAsync(listing, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} created successfully by user {OwnerId}.", listing.Id, ownerId);

        var created = await _listingRepository.GetByIdWithDetailsAsync(listing.Id, cancellationToken);
        return MapToDetailsDto(created ?? listing);
    }

    public async Task<ListingDetailsDto> UpdateAsync(Guid id, UpdateListingDto dto, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null)
            throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, isAdmin);

        // Rename & Slug update if title changed
        var slug = listing.Slug;
        if (!string.Equals(listing.Title, dto.Title, StringComparison.OrdinalIgnoreCase))
        {
            slug = await GenerateUniqueSlugAsync(dto.Title, listing.Id, cancellationToken);
        }

        listing.Rename(dto.Title, slug, dto.Description);

        if (dto.CategoryId != listing.CategoryId)
        {
            listing.ChangeCategory(dto.CategoryId);
        }

        if (dto.Quantity != listing.Quantity)
        {
            listing.UpdateQuantity(dto.Quantity);
        }

        if (dto.RequiresOwnerApproval)
        {
            listing.DisableInstantBooking();
        }
        else
        {
            listing.EnableInstantBooking();
        }

        // Address update
        if (!string.IsNullOrWhiteSpace(dto.Street) &&
            !string.IsNullOrWhiteSpace(dto.City) &&
            !string.IsNullOrWhiteSpace(dto.State) &&
            !string.IsNullOrWhiteSpace(dto.ZipCode) &&
            !string.IsNullOrWhiteSpace(dto.Country))
        {
            var address = new Address(dto.Street, dto.City, dto.State, dto.ZipCode, dto.Country);
            GeoLocation? geo = null;
            if (dto.Latitude.HasValue && dto.Longitude.HasValue)
            {
                geo = new GeoLocation(dto.Latitude.Value, dto.Longitude.Value);
            }
            listing.UpdateLocation(address, geo);
        }

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} updated by user {UserId}.", listing.Id, userId);
        return MapToDetailsDto(listing);
    }

    public async Task<bool> DeleteAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(id, cancellationToken);
        if (listing == null)
            throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.Archive();
        _listingRepository.Remove(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} deleted by user {UserId}.", id, userId);
        return true;
    }

    #endregion

    #region Workflow & State Transitions

    public async Task SubmitForApprovalAsync(Guid id, Guid userId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, false);

        listing.SubmitForApproval();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} submitted for approval.", id);
    }

    public async Task ApproveAsync(Guid id, Guid adminUserId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        listing.Approve();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} approved by admin {AdminId}.", id, adminUserId);
    }

    public async Task RejectAsync(Guid id, Guid adminUserId, string? reason = null, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        listing.Reject();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} rejected by admin {AdminId}. Reason: {Reason}", id, adminUserId, reason ?? "None");
    }

    public async Task PublishAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.Publish();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} published by user {UserId}.", id, userId);
    }

    public async Task HideAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.Hide();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} hidden by user {UserId}.", id, userId);
    }

    public async Task ArchiveAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), id);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.Archive();
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Listing {ListingId} archived by user {UserId}.", id, userId);
    }

    #endregion

    #region Images

    public async Task<ListingImageDto> AddImageAsync(
        Guid listingId,
        string imageUrl,
        bool isPrimary = false,
        int order = 0,
        Guid? userId = null,
        bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
            throw new ArgumentException("Image URL cannot be empty.", nameof(imageUrl));

        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        if (userId.HasValue)
        {
            EnsureAuthorized(listing, userId.Value, isAdmin);
        }

        var image = new ListingImage(listingId, imageUrl, isPrimary, order);
        listing.AddImage(image);

        if (isPrimary)
        {
            listing.SetCoverImage(image.Id);
        }

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Image {ImageId} added to listing {ListingId}.", image.Id, listingId);
        return MapImageDto(image);
    }

    public async Task<ListingImageDto> UploadImageAsync(
        Guid listingId,
        Stream stream,
        string fileName,
        string contentType,
        bool isPrimary = false,
        Guid? userId = null,
        bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        if (_fileStorage == null)
            throw new InvalidOperationException("File storage service is not configured.");

        var imageUrl = await _fileStorage.UploadAsync(stream, fileName, contentType, cancellationToken);
        return await AddImageAsync(listingId, imageUrl, isPrimary, 0, userId, isAdmin, cancellationToken);
    }

    public async Task RemoveImageAsync(Guid listingId, Guid imageId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.RemoveImage(imageId);
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Image {ImageId} removed from listing {ListingId}.", imageId, listingId);
    }

    public async Task SetCoverImageAsync(Guid listingId, Guid imageId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.SetCoverImage(imageId);
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Cover image set to {ImageId} for listing {ListingId}.", imageId, listingId);
    }

    #endregion

    #region Pricing Rules

    public async Task<PricingRuleDto> AddPricingRuleAsync(
        Guid listingId,
        CreatePricingRuleDto ruleDto,
        Guid userId,
        bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ruleDto);

        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        var price = new Money(ruleDto.Price, ruleDto.Currency);
        var rule = new PricingRule(listingId, ruleDto.Unit, price, ruleDto.MinDuration, ruleDto.MaxDuration);
        listing.AddPricingRule(rule);

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pricing rule {RuleId} added to listing {ListingId}.", rule.Id, listingId);
        return MapPricingRuleDto(rule);
    }

    public async Task RemovePricingRuleAsync(Guid listingId, Guid ruleId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.RemovePricingRule(ruleId);
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Pricing rule {RuleId} removed from listing {ListingId}.", ruleId, listingId);
    }

    #endregion

    #region Availability Rules & Checking

    public async Task<AvailabilityRuleDto> AddAvailabilityRuleAsync(
        Guid listingId,
        CreateAvailabilityRuleDto ruleDto,
        Guid userId,
        bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(ruleDto);

        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        var period = new RentalPeriod(ruleDto.StartDate, ruleDto.EndDate);
        var rule = new AvailabilityRule(listingId, period, ruleDto.IsAvailable);
        listing.AddAvailabilityRule(rule);

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Availability rule {RuleId} added to listing {ListingId}.", rule.Id, listingId);
        return MapAvailabilityRuleDto(rule);
    }

    public async Task RemoveAvailabilityRuleAsync(Guid listingId, Guid ruleId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        listing.RemoveAvailabilityRule(ruleId);
        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Availability rule {RuleId} removed from listing {ListingId}.", ruleId, listingId);
    }

    public async Task<ListingAvailabilityDto> CheckAvailabilityAsync(
        Guid listingId,
        DateTime startDate,
        DateTime endDate,
        int quantity = 1,
        CancellationToken cancellationToken = default)
    {
        if (startDate >= endDate)
        {
            return new ListingAvailabilityDto
            {
                ListingId = listingId,
                StartDate = startDate,
                EndDate = endDate,
                IsAvailable = false,
                AvailableQuantity = 0,
                Reason = "Start date must be before end date."
            };
        }

        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null)
            throw new NotFoundException(nameof(Listing), listingId);

        if (listing.Status != ListingStatus.Active)
        {
            return new ListingAvailabilityDto
            {
                ListingId = listingId,
                StartDate = startDate,
                EndDate = endDate,
                IsAvailable = false,
                AvailableQuantity = 0,
                Reason = $"Listing is currently {listing.Status}."
            };
        }

        if (quantity > listing.Quantity)
        {
            return new ListingAvailabilityDto
            {
                ListingId = listingId,
                StartDate = startDate,
                EndDate = endDate,
                IsAvailable = false,
                AvailableQuantity = listing.Quantity,
                Reason = $"Requested quantity {quantity} exceeds available quantity {listing.Quantity}."
            };
        }

        // Check if any negative availability rule overlaps
        var hasConflict = listing.AvailabilityRules.Any(rule =>
            !rule.IsAvailable &&
            rule.Period.StartDate < endDate &&
            startDate < rule.Period.EndDate);

        return new ListingAvailabilityDto
        {
            ListingId = listingId,
            StartDate = startDate,
            EndDate = endDate,
            IsAvailable = !hasConflict,
            AvailableQuantity = hasConflict ? 0 : listing.Quantity,
            Reason = hasConflict ? "The listing has an availability rule blocking this period." : null
        };
    }

    #endregion

    #region Policy

    public async Task UpdatePolicyAsync(
        Guid listingId,
        UpdateListingPolicyDto policyDto,
        Guid userId,
        bool isAdmin = false,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(policyDto);

        var listing = await _listingRepository.GetByIdWithDetailsAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        var cancelDesc = string.IsNullOrWhiteSpace(policyDto.CancellationPolicyDescription)
            ? $"{policyDto.CancellationPolicyName} policy"
            : policyDto.CancellationPolicyDescription;

        var cancellationPolicy = new CancellationPolicy(policyDto.CancellationPolicyName, cancelDesc);
        Money? securityDeposit = policyDto.SecurityDepositAmount.HasValue
            ? new Money(policyDto.SecurityDepositAmount.Value, policyDto.SecurityDepositCurrency ?? "USD")
            : null;
        Money? lateFee = policyDto.LateFeeAmount.HasValue
            ? new Money(policyDto.LateFeeAmount.Value, policyDto.LateFeeCurrency ?? "USD")
            : null;

        if (listing.Policy == null)
        {
            var policy = new ListingPolicy(
                listingId,
                cancellationPolicy,
                securityDeposit,
                policyDto.DamagePolicy,
                lateFee,
                policyDto.ReturnInstructions);

            listing.SetPolicy(policy);
        }
        else
        {
            listing.Policy.UpdatePolicies(
                cancellationPolicy,
                securityDeposit,
                policyDto.DamagePolicy,
                lateFee,
                policyDto.ReturnInstructions);
        }

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Policies updated for listing {ListingId}.", listingId);
    }

    #endregion

    #region Settings & Metrics

    public async Task SetInstantBookingAsync(Guid listingId, bool enableInstantBooking, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing == null) throw new NotFoundException(nameof(Listing), listingId);

        EnsureAuthorized(listing, userId, isAdmin);

        if (enableInstantBooking)
            listing.EnableInstantBooking();
        else
            listing.DisableInstantBooking();

        _listingRepository.Update(listing);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Instant booking set to {Status} for listing {ListingId}.", enableInstantBooking, listingId);
    }

    public async Task IncrementViewCountAsync(Guid listingId, CancellationToken cancellationToken = default)
    {
        var listing = await _listingRepository.GetByIdAsync(listingId, cancellationToken);
        if (listing != null)
        {
            // ViewCount is tracked; we can increment it safely
            var property = typeof(Listing).GetProperty(nameof(Listing.ViewCount));
            if (property != null)
            {
                property.SetValue(listing, listing.ViewCount + 1);
                _listingRepository.Update(listing);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }

    #endregion

    #region Private Helpers

    private static void EnsureAuthorized(Listing listing, Guid userId, bool isAdmin)
    {
        if (!isAdmin && listing.OwnerId != userId)
        {
            throw new UnAuthorizedException("You are not authorized to modify this listing.");
        }
    }

    private async Task<string> GenerateUniqueSlugAsync(string title, Guid? excludeId, CancellationToken cancellationToken)
    {
        var baseSlug = _slugGenerator.Generate(title);
        var slug = baseSlug;
        var counter = 1;

        while (await _listingRepository.SlugExistsAsync(slug, excludeId, cancellationToken))
        {
            slug = $"{baseSlug}-{counter++}";
        }

        return slug;
    }

    private IQueryable<Listing> BuildFilteredQuery(ListingFilterDto filter)
    {
        var query = _listingRepository.QueryableNoTracking()
            .Include(l => l.Images)
            .Include(l => l.Category)
            .Include(l => l.Owner)
            .Include(l => l.PricingRules)
            .AsQueryable();

        // Status
        if (filter.Status.HasValue)
        {
            query = query.Where(l => l.Status == filter.Status.Value);
        }
        else
        {
            // By default, public search/browse shows active listings
            query = query.Where(l => l.Status == ListingStatus.Active);
        }

        // Category
        if (filter.CategoryId.HasValue)
        {
            query = query.Where(l => l.CategoryId == filter.CategoryId.Value);
        }

        // Owner
        if (filter.OwnerId.HasValue)
        {
            query = query.Where(l => l.OwnerId == filter.OwnerId.Value);
        }

        // Condition
        if (filter.Condition.HasValue)
        {
            query = query.Where(l => l.Condition == filter.Condition.Value);
        }

        // Delivery Option
        if (filter.DeliveryOption.HasValue)
        {
            query = query.Where(l => l.DeliveryOption == filter.DeliveryOption.Value);
        }

        // City & Country
        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            query = query.Where(l => l.Address != null && l.Address.City.ToLower().Contains(filter.City.Trim().ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            query = query.Where(l => l.Address != null && l.Address.Country.ToLower().Contains(filter.Country.Trim().ToLower()));
        }

        // Search term (title, description, brand, model)
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            var term = filter.SearchTerm.Trim().ToLower();
            query = query.Where(l =>
                l.Title.ToLower().Contains(term) ||
                l.Description.ToLower().Contains(term) ||
                (l.Brand != null && l.Brand.ToLower().Contains(term)) ||
                (l.Model != null && l.Model.ToLower().Contains(term)));
        }

        // Price filtering
        if (filter.MinPrice.HasValue || filter.MaxPrice.HasValue || filter.RentalUnit.HasValue)
        {
            query = query.Where(l => l.PricingRules.Any(pr =>
                (!filter.RentalUnit.HasValue || pr.Unit == filter.RentalUnit.Value) &&
                (!filter.MinPrice.HasValue || pr.Price.Amount >= filter.MinPrice.Value) &&
                (!filter.MaxPrice.HasValue || pr.Price.Amount <= filter.MaxPrice.Value)));
        }

        // Sorting
        query = filter.SortBy?.ToLowerInvariant() switch
        {
            "price_asc" => query.OrderBy(l => l.PricingRules.Min(pr => (decimal?)pr.Price.Amount)),
            "price_desc" => query.OrderByDescending(l => l.PricingRules.Max(pr => (decimal?)pr.Price.Amount)),
            "rating" => query.OrderByDescending(l => l.AverageRating).ThenByDescending(l => l.ReviewCount),
            "views" => query.OrderByDescending(l => l.ViewCount),
            "popular" => query.OrderByDescending(l => l.BookingCount),
            "oldest" => query.OrderBy(l => l.CreatedAt),
            _ => query.OrderByDescending(l => l.CreatedAt) // "newest" default
        };

        return query;
    }

    private static ListingCardDto MapToCardDto(Listing listing)
    {
        var primaryImage = listing.Images.FirstOrDefault(i => i.IsPrimary) ?? listing.Images.FirstOrDefault();
        var primaryPrice = listing.PricingRules.OrderBy(pr => pr.Unit).FirstOrDefault();

        return new ListingCardDto
        {
            Id = listing.Id,
            Title = listing.Title,
            Slug = listing.Slug,
            CategoryId = listing.CategoryId,
            CategoryName = listing.Category?.Name ?? string.Empty,
            PrimaryImageUrl = primaryImage?.ImageUrl,
            BasePrice = primaryPrice?.Price.Amount,
            BasePriceUnit = primaryPrice?.Unit,
            Currency = primaryPrice?.Price.Currency ?? "USD",
            City = listing.Address?.City,
            State = listing.Address?.State,
            Country = listing.Address?.Country,
            Condition = listing.Condition,
            Status = listing.Status,
            AverageRating = listing.AverageRating,
            ReviewCount = listing.ReviewCount,
            RequiresOwnerApproval = listing.RequiresOwnerApproval,
            DeliveryOption = listing.DeliveryOption,
            OwnerId = listing.OwnerId,
            OwnerName = listing.Owner?.FullName ?? string.Empty
        };
    }

    private static ListingDto MapToDto(Listing listing)
    {
        var coverImage = listing.CoverImageId.HasValue
            ? listing.Images.FirstOrDefault(i => i.Id == listing.CoverImageId.Value)
            : listing.Images.FirstOrDefault(i => i.IsPrimary) ?? listing.Images.FirstOrDefault();

        return new ListingDto
        {
            Id = listing.Id,
            OwnerId = listing.OwnerId,
            OwnerName = listing.Owner?.FullName ?? string.Empty,
            CategoryId = listing.CategoryId,
            CategoryName = listing.Category?.Name ?? string.Empty,
            Title = listing.Title,
            Slug = listing.Slug,
            Description = listing.Description,
            Brand = listing.Brand,
            Model = listing.Model,
            Condition = listing.Condition,
            Year = listing.Year,
            Quantity = listing.Quantity,
            Status = listing.Status,
            DeliveryOption = listing.DeliveryOption,
            RequiresOwnerApproval = listing.RequiresOwnerApproval,
            Street = listing.Address?.Street,
            City = listing.Address?.City,
            State = listing.Address?.State,
            ZipCode = listing.Address?.ZipCode,
            Country = listing.Address?.Country,
            Latitude = listing.GeoLocation?.Latitude,
            Longitude = listing.GeoLocation?.Longitude,
            CoverImageId = listing.CoverImageId,
            CoverImageUrl = coverImage?.ImageUrl,
            ViewCount = listing.ViewCount,
            BookingCount = listing.BookingCount,
            AverageRating = listing.AverageRating,
            ReviewCount = listing.ReviewCount,
            CreatedAt = listing.CreatedAt
        };
    }

    private static ListingDetailsDto MapToDetailsDto(Listing listing)
    {
        var baseDto = MapToDto(listing);

        return new ListingDetailsDto
        {
            Id = baseDto.Id,
            OwnerId = baseDto.OwnerId,
            OwnerName = baseDto.OwnerName,
            CategoryId = baseDto.CategoryId,
            CategoryName = baseDto.CategoryName,
            Title = baseDto.Title,
            Slug = baseDto.Slug,
            Description = baseDto.Description,
            Brand = baseDto.Brand,
            Model = baseDto.Model,
            Condition = baseDto.Condition,
            Year = baseDto.Year,
            Quantity = baseDto.Quantity,
            Status = baseDto.Status,
            DeliveryOption = baseDto.DeliveryOption,
            RequiresOwnerApproval = baseDto.RequiresOwnerApproval,
            Street = baseDto.Street,
            City = baseDto.City,
            State = baseDto.State,
            ZipCode = baseDto.ZipCode,
            Country = baseDto.Country,
            Latitude = baseDto.Latitude,
            Longitude = baseDto.Longitude,
            CoverImageId = baseDto.CoverImageId,
            CoverImageUrl = baseDto.CoverImageUrl,
            ViewCount = baseDto.ViewCount,
            BookingCount = baseDto.BookingCount,
            AverageRating = baseDto.AverageRating,
            ReviewCount = baseDto.ReviewCount,
            CreatedAt = baseDto.CreatedAt,
            MetaTitle = listing.MetaTitle,
            MetaDescription = listing.MetaDescription,
            OwnerEmail = listing.Owner?.Email,
            OwnerPhoneNumber = listing.Owner?.PhoneNumber,
            Policy = listing.Policy == null ? null : MapPolicyDto(listing.Policy),
            Images = listing.Images.OrderBy(i => i.Order).Select(MapImageDto).ToList(),
            PricingRules = listing.PricingRules.Select(MapPricingRuleDto).ToList(),
            AvailabilityRules = listing.AvailabilityRules.Select(MapAvailabilityRuleDto).ToList()
        };
    }

    private static ListingImageDto MapImageDto(ListingImage image)
    {
        return new ListingImageDto
        {
            Id = image.Id,
            ListingId = image.ListingId,
            ImageUrl = image.ImageUrl,
            IsPrimary = image.IsPrimary,
            Order = image.Order
        };
    }

    private static PricingRuleDto MapPricingRuleDto(PricingRule rule)
    {
        return new PricingRuleDto
        {
            Id = rule.Id,
            ListingId = rule.ListingId,
            Unit = rule.Unit,
            Price = rule.Price.Amount,
            Currency = rule.Price.Currency,
            MinDuration = rule.MinDuration,
            MaxDuration = rule.MaxDuration
        };
    }

    private static AvailabilityRuleDto MapAvailabilityRuleDto(AvailabilityRule rule)
    {
        return new AvailabilityRuleDto
        {
            Id = rule.Id,
            ListingId = rule.ListingId,
            StartDate = rule.Period.StartDate,
            EndDate = rule.Period.EndDate,
            IsAvailable = rule.IsAvailable
        };
    }

    private static ListingPolicyDto MapPolicyDto(ListingPolicy policy)
    {
        return new ListingPolicyDto
        {
            Id = policy.Id,
            ListingId = policy.ListingId,
            CancellationPolicyName = policy.CancellationPolicy.Name,
            CancellationPolicyDescription = policy.CancellationPolicy.Description,
            SecurityDepositAmount = policy.SecurityDeposit?.Amount,
            SecurityDepositCurrency = policy.SecurityDeposit?.Currency,
            DamagePolicy = policy.DamagePolicy,
            LateFeeAmount = policy.LateFee?.Amount,
            LateFeeCurrency = policy.LateFee?.Currency,
            ReturnInstructions = policy.ReturnInstructions
        };
    }

    #endregion
}
