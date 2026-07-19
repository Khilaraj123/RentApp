using System;
using System.Linq;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents;
using RentApp.Domain.Entities.Categories;
using RentApp.Domain.Entities.Users;

namespace RentApp.Domain.Entities.Listings
{
    public class Listing : SoftDeleteEntity
    {
        public Guid OwnerId { get; private set; }
        public virtual ApplicationUser? Owner { get; private set; }
        
        public Guid CategoryId { get; private set; }
        public virtual Category? Category { get; private set; }
        
        public string Title { get; private set; } = string.Empty;
        public string Slug { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
        public ListingCondition Condition { get; private set; }
        public int? Year { get; private set; }
        public int Quantity { get; private set; }
        
        public Address? Address { get; private set; }
        public GeoLocation? GeoLocation { get; private set; }
        
        public ListingStatus Status { get; private set; }
        public DeliveryOption DeliveryOption { get; private set; }
        
        public bool RequiresOwnerApproval { get; private set; } = true;
        
        public Guid? CoverImageId { get; private set; }
        public string? MetaTitle { get; private set; }
        public string? MetaDescription { get; private set; }

        public int ViewCount { get; private set; }
        public int BookingCount { get; private set; }
        public decimal AverageRating { get; private set; }
        public int ReviewCount { get; private set; }

        public virtual ListingPolicy? Policy { get; private set; }

        private readonly List<ListingImage> _images = new();
        public IReadOnlyCollection<ListingImage> Images => _images.AsReadOnly();

        private readonly List<AvailabilityRule> _availabilityRules = new();
        public IReadOnlyCollection<AvailabilityRule> AvailabilityRules => _availabilityRules.AsReadOnly();

        private readonly List<PricingRule> _pricingRules = new();
        public IReadOnlyCollection<PricingRule> PricingRules => _pricingRules.AsReadOnly();

        private Listing() { } // EF Core

        public Listing(
            Guid ownerId, 
            Guid categoryId, 
            string title, 
            string slug, 
            string description, 
            ListingCondition condition, 
            int quantity,
            DeliveryOption deliveryOption)
        {
            if (ownerId == Guid.Empty) throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));
            if (categoryId == Guid.Empty) throw new ArgumentException("CategoryId cannot be empty.", nameof(categoryId));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentException("Slug cannot be empty.", nameof(slug));
            if (quantity <= 0) throw new ArgumentException("Quantity must be greater than zero.", nameof(quantity));

            OwnerId = ownerId;
            CategoryId = categoryId;
            Title = title.Trim();
            Slug = slug.Trim().ToLowerInvariant();
            Description = description?.Trim() ?? string.Empty;
            Condition = condition;
            Quantity = quantity;
            DeliveryOption = deliveryOption;
            Status = ListingStatus.Draft;
        }

        public void SetPolicy(ListingPolicy policy)
        {
            if (policy == null) throw new ArgumentNullException(nameof(policy));
            Policy = policy;
        }

        public void Rename(string title, string slug, string description)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(slug)) throw new ArgumentException("Slug cannot be empty.", nameof(slug));

            Title = title.Trim();
            Slug = slug.Trim().ToLowerInvariant();
            Description = description?.Trim() ?? string.Empty;
        }

        public void ChangeCategory(Guid categoryId)
        {
            if (categoryId == Guid.Empty) throw new ArgumentException("CategoryId cannot be empty.", nameof(categoryId));
            CategoryId = categoryId;
        }

        public void UpdateLocation(Address address, GeoLocation? geoLocation)
        {
            Address = address ?? throw new ArgumentNullException(nameof(address));
            GeoLocation = geoLocation;
        }

        public void UpdateQuantity(int quantity)
        {
            if (quantity < 0) throw new ArgumentException("Quantity cannot be negative.", nameof(quantity));
            Quantity = quantity;
        }
        
        public void SetCoverImage(Guid imageId)
        {
            if (!_images.Any(x => x.Id == imageId))
                throw new InvalidOperationException("Image does not exist in the listing.");
                
            CoverImageId = imageId;
        }

        public void EnableInstantBooking()
        {
            RequiresOwnerApproval = false;
        }

        public void DisableInstantBooking()
        {
            RequiresOwnerApproval = true;
        }

        // --- Workflow ---

        public void SubmitForApproval()
        {
            if (Status != ListingStatus.Draft && Status != ListingStatus.Rejected)
                throw new InvalidOperationException($"Cannot submit for approval from status {Status}.");

            ValidatePublishableState();
            Status = ListingStatus.PendingApproval;
        }

        public void Approve()
        {
            if (Status != ListingStatus.PendingApproval)
                throw new InvalidOperationException($"Cannot approve from status {Status}.");

            Status = ListingStatus.Active;
            AddDomainEvent(new ListingApprovedEvent(Id));
            AddDomainEvent(new ListingPublishedEvent(Id));
        }

        public void Reject()
        {
            if (Status != ListingStatus.PendingApproval)
                throw new InvalidOperationException($"Cannot reject from status {Status}.");

            Status = ListingStatus.Rejected;
            AddDomainEvent(new ListingRejectedEvent(Id));
        }

        public void Publish()
        {
            if (Status != ListingStatus.Draft && Status != ListingStatus.Inactive && Status != ListingStatus.Rejected)
                throw new InvalidOperationException($"Cannot directly publish from status {Status}.");

            ValidatePublishableState();
            Status = ListingStatus.Active;
            AddDomainEvent(new ListingPublishedEvent(Id));
        }

        public void Hide()
        {
            if (Status != ListingStatus.Active)
                throw new InvalidOperationException($"Cannot hide from status {Status}.");

            Status = ListingStatus.Inactive;
            AddDomainEvent(new ListingHiddenEvent(Id));
        }

        public void Archive()
        {
            Status = ListingStatus.Inactive; // or a dedicated Archived status if added to enum
            AddDomainEvent(new ListingArchivedEvent(Id));
        }

        private void ValidatePublishableState()
        {
            if (!_images.Any())
                throw new InvalidOperationException("Cannot publish a listing without images.");
            if (!_pricingRules.Any())
                throw new InvalidOperationException("Cannot publish a listing without pricing rules.");
            if (Address == null)
                throw new InvalidOperationException("Cannot publish a listing without an address.");
            if (Quantity <= 0)
                throw new InvalidOperationException("Cannot publish a listing with zero quantity.");
            if (Policy == null)
                throw new InvalidOperationException("Cannot publish a listing without policies.");
        }

        // --- Collections ---

        public void AddImage(ListingImage image)
        {
            if (image == null) throw new ArgumentNullException(nameof(image));
            if (_images.Any(x => x.Id == image.Id)) return;

            _images.Add(image);
            if (CoverImageId == null)
            {
                CoverImageId = image.Id;
            }
        }

        public void RemoveImage(Guid imageId)
        {
            var image = _images.FirstOrDefault(x => x.Id == imageId);
            if (image != null)
            {
                _images.Remove(image);
                if (CoverImageId == imageId)
                {
                    CoverImageId = _images.FirstOrDefault()?.Id;
                }
            }
        }

        public void AddPricingRule(PricingRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            if (_pricingRules.Any(x => x.Unit == rule.Unit))
                throw new InvalidOperationException($"A pricing rule for {rule.Unit} already exists.");

            _pricingRules.Add(rule);
        }

        public void RemovePricingRule(Guid ruleId)
        {
            var rule = _pricingRules.FirstOrDefault(x => x.Id == ruleId);
            if (rule != null)
                _pricingRules.Remove(rule);
        }

        public void AddAvailabilityRule(AvailabilityRule rule)
        {
            if (rule == null) throw new ArgumentNullException(nameof(rule));
            
            // Check overlaps
            if (_availabilityRules.Any(x => x.Period.StartDate < rule.Period.EndDate && rule.Period.StartDate < x.Period.EndDate))
                throw new InvalidOperationException("Availability rule overlaps with an existing rule.");

            _availabilityRules.Add(rule);
        }

        public void RemoveAvailabilityRule(Guid ruleId)
        {
            var rule = _availabilityRules.FirstOrDefault(x => x.Id == ruleId);
            if (rule != null)
                _availabilityRules.Remove(rule);
        }
    }
}
