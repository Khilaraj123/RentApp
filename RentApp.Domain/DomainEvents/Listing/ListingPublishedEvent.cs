using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Listing
{
    public class ListingPublishedEvent : DomainEvent
    {
        public Guid ListingId { get; }

        public ListingPublishedEvent(Guid listingId)
        {
            ListingId = listingId;
        }
    }
}
