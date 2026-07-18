using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Listings
{
    public class AvailabilityRule : BaseEntity
    {
        public Guid ListingId { get; private set; }
        public RentalPeriod Period { get; private set; } = null!;
        public bool IsAvailable { get; private set; }

        private AvailabilityRule() { } // EF Core

        public AvailabilityRule(Guid listingId, RentalPeriod period, bool isAvailable)
        {
            ListingId = listingId;
            Period = period;
            IsAvailable = isAvailable;
        }
    }
}
