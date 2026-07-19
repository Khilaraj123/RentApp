using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class RefundRequestedEvent : DomainEvent
    {
        public Guid RefundId { get; }
        public Guid PaymentId { get; }

        public RefundRequestedEvent(Guid refundId, Guid paymentId)
        {
            RefundId = refundId;
            PaymentId = paymentId;
        }
    }
}
