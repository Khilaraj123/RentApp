using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class PaymentFailedEvent : DomainEvent
    {
        public Guid PaymentId { get; }
        public Guid BookingId { get; }
        public string? FailureReason { get; }

        public PaymentFailedEvent(Guid paymentId, Guid bookingId, string? failureReason)
        {
            PaymentId = paymentId;
            BookingId = bookingId;
            FailureReason = failureReason;
        }
    }
}
