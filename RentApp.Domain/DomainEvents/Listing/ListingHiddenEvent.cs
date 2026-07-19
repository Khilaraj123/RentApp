using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ListingHiddenEvent : DomainEvent
    {
        public Guid ListingId { get; }

        public ListingHiddenEvent(Guid listingId)
        {
            ListingId = listingId;
        }
    }
}
