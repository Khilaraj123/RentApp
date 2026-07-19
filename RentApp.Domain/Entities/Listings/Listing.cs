using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Listings
{
    public class Listing : SoftDeleteEntity
    {
        public Guid OwnerId { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public Guid CategoryId { get; private set; }
        public string? Brand { get; private set; }
        public string? Model { get; private set; }
        public ListingCondition Condition { get; private set; }
        public int? Year { get; private set; }
        public int Quantity { get; private set; }
        
        public Address? Address { get; private set; }
        public GeoLocation? GeoLocation { get; private set; }
        
        public ListingStatus Status { get; private set; }
        public CancellationPolicy CancellationPolicy { get; private set; } = CancellationPolicy.Moderate;
        public Money? SecurityDeposit { get; private set; }
        
        public bool RequiresOwnerApproval { get; private set; } = true;

        private readonly List<ListingImage> _images = new();
        public IReadOnlyCollection<ListingImage> Images => _images.AsReadOnly();

        private readonly List<AvailabilityRule> _availabilityRules = new();
        public IReadOnlyCollection<AvailabilityRule> AvailabilityRules => _availabilityRules.AsReadOnly();

        private readonly List<PricingRule> _pricingRules = new();
        public IReadOnlyCollection<PricingRule> PricingRules => _pricingRules.AsReadOnly();

        private Listing() { } // EF Core

        public Listing(Guid ownerId, string title, string description, Guid categoryId, ListingCondition condition, int quantity)
        {
            OwnerId = ownerId;
            Title = title;
            Description = description;
            CategoryId = categoryId;
            Condition = condition;
            Quantity = quantity;
            Status = ListingStatus.Draft;
        }

        public void Publish()
        {
            Status = ListingStatus.Active;
        }

        public void Archive()
        {
            Status = ListingStatus.Inactive;
        }

        public void AddImage(ListingImage image)
        {
            _images.Add(image);
        }

        public void AddPricingRule(PricingRule rule)
        {
            _pricingRules.Add(rule);
        }

        public void AddAvailabilityRule(AvailabilityRule rule)
        {
            _availabilityRules.Add(rule);
        }
    }
}
