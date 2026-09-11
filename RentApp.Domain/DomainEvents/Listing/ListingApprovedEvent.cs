using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Listing
{
    public class ListingApprovedEvent : DomainEvent
    {
        public Guid ListingId { get; }

        public ListingApprovedEvent(Guid listingId)
        {
            ListingId = listingId;
        }
    }
}
