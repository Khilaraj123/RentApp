using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
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
