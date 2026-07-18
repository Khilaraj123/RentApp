using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class Payment : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public PaymentMethod Method { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? TransactionId { get; private set; }
        public string? GatewayResponse { get; private set; }

        private Payment() { } // EF Core

        public Payment(Guid bookingId, Money amount, PaymentMethod method)
        {
            BookingId = bookingId;
            Amount = amount;
            Method = method;
            Status = PaymentStatus.Pending;
        }

        public void MarkAsCompleted(string transactionId, string? gatewayResponse)
        {
            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
            GatewayResponse = gatewayResponse;
        }

        public void MarkAsFailed(string? gatewayResponse)
        {
            Status = PaymentStatus.Failed;
            GatewayResponse = gatewayResponse;
        }
    }
}
