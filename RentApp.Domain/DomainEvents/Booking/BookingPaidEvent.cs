using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Booking
{
    public class BookingPaidEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingPaidEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
