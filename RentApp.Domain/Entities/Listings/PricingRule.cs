using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Listings
{
    public class PricingRule : BaseEntity
    {
        public Guid ListingId { get; private set; }
        public string Type { get; private set; } = string.Empty; // PerDay, PerHour, PerWeek, PerMonth
        public Money Price { get; private set; } = null!;
        public int? MinDuration { get; private set; }
        public int? MaxDuration { get; private set; }

        private PricingRule() { } // EF Core

        public PricingRule(Guid listingId, string type, Money price, int? minDuration, int? maxDuration)
        {
            ListingId = listingId;
            Type = type;
            Price = price;
            MinDuration = minDuration;
            MaxDuration = maxDuration;
        }
    }
}
