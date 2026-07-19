using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class RefundCompletedEvent : DomainEvent
    {
        public Guid RefundId { get; }

        public RefundCompletedEvent(Guid refundId)
        {
            RefundId = refundId;
        }
    }
}
