using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class BookingCompletedEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public BookingCompletedEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
