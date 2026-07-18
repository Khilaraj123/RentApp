using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ItemPickedUpEvent : DomainEvent
    {
        public Guid BookingId { get; }

        public ItemPickedUpEvent(Guid bookingId)
        {
            BookingId = bookingId;
        }
    }
}
