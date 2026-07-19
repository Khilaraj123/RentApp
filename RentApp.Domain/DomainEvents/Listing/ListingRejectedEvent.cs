using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
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
