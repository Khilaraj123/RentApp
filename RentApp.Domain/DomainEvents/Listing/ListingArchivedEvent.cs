using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ListingArchivedEvent : DomainEvent
    {
        public Guid ListingId { get; }

        public ListingArchivedEvent(Guid listingId)
        {
            ListingId = listingId;
        }
    }
}
