using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class RefundApprovedEvent : DomainEvent
    {
        public Guid RefundId { get; }

        public RefundApprovedEvent(Guid refundId)
        {
            RefundId = refundId;
        }
    }
}
