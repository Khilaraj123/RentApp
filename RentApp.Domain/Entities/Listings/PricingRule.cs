using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Listings
{
    public class PricingRule : BaseEntity
    {
        public Guid ListingId { get; private set; }
        public RentalUnit Unit { get; private set; }
        public Money Price { get; private set; } = null!;
        public int? MinDuration { get; private set; }
        public int? MaxDuration { get; private set; }

        private PricingRule() { } // EF Core

        public PricingRule(Guid listingId, RentalUnit unit, Money price, int? minDuration, int? maxDuration)
        {
            if (minDuration.HasValue && maxDuration.HasValue && minDuration > maxDuration)
                throw new ArgumentException("Minimum duration cannot be greater than maximum duration.");

            ListingId = listingId;
            Unit = unit;
            Price = price;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
        }
    }
}
