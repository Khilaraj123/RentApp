using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Booking
{
    public class BookingConfirmedEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingConfirmedEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
