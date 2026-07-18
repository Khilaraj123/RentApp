using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ItemReturnedEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public ItemReturnedEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
