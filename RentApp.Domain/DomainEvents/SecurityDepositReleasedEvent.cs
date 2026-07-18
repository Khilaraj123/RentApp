using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class SecurityDepositReleasedEvent : DomainEvent
    {
        public Guid BookingId { get; }
        public Guid? RefundId { get; }

        public SecurityDepositReleasedEvent(Guid bookingId, Guid? refundId)
        {
            BookingId = bookingId;
            RefundId = refundId;
        }
    }
}
