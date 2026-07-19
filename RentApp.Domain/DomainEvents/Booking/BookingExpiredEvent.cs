using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class BookingExpiredEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingExpiredEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
