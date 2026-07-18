using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class Refund : AuditableEntity
    {
        public Guid PaymentId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public string? Reason { get; private set; }
        public PaymentStatus Status { get; private set; }
        public string? TransactionId { get; private set; }

        private Refund() { } // EF Core

        public Refund(Guid paymentId, Money amount, string? reason)
        {
            PaymentId = paymentId;
            Amount = amount;
            Reason = reason;
            Status = PaymentStatus.Pending;
        }

        public void MarkAsCompleted(string transactionId)
        {
            Status = PaymentStatus.Completed;
            TransactionId = transactionId;
        }

        public void MarkAsFailed()
        {
            Status = PaymentStatus.Failed;
        }
    }
}
