using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class DisputeOpenedEvent : DomainEvent
    {
        public Guid DisputeId { get; }
        public Guid BookingId { get; }

        public DisputeOpenedEvent(Guid disputeId, Guid bookingId)
        {
            DisputeId = disputeId;
            BookingId = bookingId;
        }
    }
}
