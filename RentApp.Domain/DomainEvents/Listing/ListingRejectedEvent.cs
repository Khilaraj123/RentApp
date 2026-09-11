using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Listing
{
    public class ListingRejectedEvent : DomainEvent
    {
        public Guid ListingId { get; }

        public ListingRejectedEvent(Guid listingId)
        {
            ListingId = listingId;
        }
    }
}
