using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using RentApp.Application.DTOs.Listings;
using RentApp.Domain.Common.Pagination;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.Interfaces.Listings;

public interface IListingService
{
    // Queries
    Task<ListingDetailsDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ListingDetailsDto?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingCardDto>> GetPagedListingsAsync(ListingFilterDto filter, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingCardDto>> SearchListingsAsync(ListingSearchDto search, CancellationToken cancellationToken = default);
    Task<PagedResult<ListingCardDto>> GetOwnerListingsAsync(Guid ownerId, PaginationRequest request, ListingStatus? status = null, CancellationToken cancellationToken = default);
    Task<List<ListingCardDto>> GetFeaturedListingsAsync(int count = 6, CancellationToken cancellationToken = default);
    Task<List<ListingCardDto>> GetRecentListingsAsync(int count = 6, CancellationToken cancellationToken = default);
    Task<List<ListingCardDto>> GetSimilarListingsAsync(Guid listingId, int count = 4, CancellationToken cancellationToken = default);

    // CRUD
    Task<ListingDetailsDto> CreateAsync(CreateListingDto dto, Guid ownerId, CancellationToken cancellationToken = default);
    Task<ListingDetailsDto> UpdateAsync(Guid id, UpdateListingDto dto, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    // Workflow & State Transitions
    Task SubmitForApprovalAsync(Guid id, Guid userId, CancellationToken cancellationToken = default);
    Task ApproveAsync(Guid id, Guid adminUserId, CancellationToken cancellationToken = default);
    Task RejectAsync(Guid id, Guid adminUserId, string? reason = null, CancellationToken cancellationToken = default);
    Task PublishAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task HideAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task ArchiveAsync(Guid id, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    // Images
    Task<ListingImageDto> AddImageAsync(Guid listingId, string imageUrl, bool isPrimary = false, int order = 0, Guid? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task<ListingImageDto> UploadImageAsync(Guid listingId, Stream stream, string fileName, string contentType, bool isPrimary = false, Guid? userId = null, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task RemoveImageAsync(Guid listingId, Guid imageId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task SetCoverImageAsync(Guid listingId, Guid imageId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    // Pricing Rules
    Task<PricingRuleDto> AddPricingRuleAsync(Guid listingId, CreatePricingRuleDto ruleDto, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task RemovePricingRuleAsync(Guid listingId, Guid ruleId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    // Availability Rules
    Task<AvailabilityRuleDto> AddAvailabilityRuleAsync(Guid listingId, CreateAvailabilityRuleDto ruleDto, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task RemoveAvailabilityRuleAsync(Guid listingId, Guid ruleId, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task<ListingAvailabilityDto> CheckAvailabilityAsync(Guid listingId, DateTime startDate, DateTime endDate, int quantity = 1, CancellationToken cancellationToken = default);

    // Policy
    Task UpdatePolicyAsync(Guid listingId, UpdateListingPolicyDto policyDto, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);

    // Settings & Metrics
    Task SetInstantBookingAsync(Guid listingId, bool enableInstantBooking, Guid userId, bool isAdmin = false, CancellationToken cancellationToken = default);
    Task IncrementViewCountAsync(Guid listingId, CancellationToken cancellationToken = default);
}
