using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class BookingRejectedEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingRejectedEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
