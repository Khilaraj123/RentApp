using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class BookingCancelledEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingCancelledEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
