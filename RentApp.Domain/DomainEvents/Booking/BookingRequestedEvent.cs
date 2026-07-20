using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class BookingRequestedEvent : DomainEvent
    {
        public Guid BookingId { get; }
        public Guid ListingId { get; }
        public Guid RenterId { get; }

        public BookingRequestedEvent(Guid bookingId, Guid listingId, Guid renterId)
        {
            BookingId = bookingId;
            ListingId = listingId;
            RenterId = renterId;
        }
    }
}
