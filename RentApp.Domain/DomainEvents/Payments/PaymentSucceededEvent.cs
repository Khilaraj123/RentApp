using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class PaymentSucceededEvent : DomainEvent
    {
        public Guid PaymentId { get; }
        public Guid BookingId { get; }

        public PaymentSucceededEvent(Guid paymentId, Guid bookingId)
        {
            PaymentId = paymentId;
            BookingId = bookingId;
        }
    }
}
