using System;
using System.Collections.Generic;
using System.Linq;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Payments
{
    public class Payment : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public PaymentMethod Method { get; private set; }
        public PaymentGateway Gateway { get; private set; }
        
        public PaymentStatus Status { get; private set; }
        
        public string? TransactionId { get; private set; }
        public string? PaymentReference { get; private set; }
        public string? PaymentIntentId { get; private set; }
        
        public DateTime? PaidAtUtc { get; private set; }
        public string? FailureReason { get; private set; }
        
        // Navigation properties if any
        public string? GatewayResponse { get; private set; }

        private readonly List<Refund> _refunds = new();
        public IReadOnlyCollection<Refund> Refunds => _refunds.AsReadOnly();

        private Payment() { } // EF Core

        public Payment(Guid bookingId, Money amount, PaymentMethod method, PaymentGateway gateway, string? paymentIntentId = null)
        {
            if (bookingId == Guid.Empty) throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
            if (amount == null) throw new ArgumentNullException(nameof(amount));
            if (amount.Amount <= 0) throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));

            BookingId = bookingId;
            Amount = amount;
            Method = method;
            Gateway = gateway;
            PaymentIntentId = paymentIntentId;
            
            Status = PaymentStatus.Pending;
        }

        public void MarkAsCompleted(string transactionId, string? paymentReference = null, string? gatewayResponse = null)
        {
            if (Status == PaymentStatus.Completed) return; // Idempotent

            if (string.IsNullOrWhiteSpace(transactionId)) 
                throw new ArgumentException("TransactionId is required to complete a payment.", nameof(transactionId));

            Status = PaymentStatus.Completed;
            TransactionId = transactionId.Trim();
            PaymentReference = paymentReference?.Trim();
            GatewayResponse = gatewayResponse;
            PaidAtUtc = DateTime.UtcNow;

            AddDomainEvent(new PaymentSucceededEvent(Id, BookingId));
        }

        public void MarkAsFailed(string? failureReason, string? gatewayResponse = null)
        {
            if (Status == PaymentStatus.Failed) return; // Idempotent
            if (Status == PaymentStatus.Completed) 
                throw new InvalidOperationException("Cannot fail a payment that has already completed.");

            Status = PaymentStatus.Failed;
            FailureReason = failureReason;
            GatewayResponse = gatewayResponse;

            AddDomainEvent(new PaymentFailedEvent(Id, BookingId, failureReason));
        }

        public Refund RequestRefund(Money amountToRefund, string? reason, RefundType type)
        {
            if (Status != PaymentStatus.Completed)
            {
                throw new InvalidOperationException("Cannot refund a payment that is not completed.");
            }

            if (amountToRefund == null) throw new ArgumentNullException(nameof(amountToRefund));
            if (amountToRefund.Currency != Amount.Currency)
            {
                throw new ArgumentException("Refund currency must match payment currency.");
            }

            decimal totalRefunded = _refunds
                .Where(r => r.Status != RefundStatus.Rejected && r.Status != RefundStatus.Failed)
                .Sum(r => r.Amount.Amount);

            if (totalRefunded + amountToRefund.Amount > Amount.Amount)
            {
                throw new InvalidOperationException("Total refunds cannot exceed the original payment amount.");
            }

            var refund = new Refund(Id, amountToRefund, reason, type);
            _refunds.Add(refund);

            AddDomainEvent(new RefundRequestedEvent(refund.Id, Id));

            return refund;
        }
    }
}
