using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Payments
{
    public class Refund : AuditableEntity
    {
        public Guid PaymentId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public string? Reason { get; private set; }
        
        public RefundStatus Status { get; private set; }
        public RefundType Type { get; private set; }
        
        public string? TransactionId { get; private set; }

        public Guid? ApprovedByUserId { get; private set; }
        public DateTime? ApprovedAtUtc { get; private set; }
        public DateTime? RefundedAtUtc { get; private set; }

        private Refund() { } // EF Core

        internal Refund(Guid paymentId, Money amount, string? reason, RefundType type)
        {
            if (paymentId == Guid.Empty) throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
            if (amount == null) throw new ArgumentNullException(nameof(amount));
            if (amount.Amount <= 0) throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));

            PaymentId = paymentId;
            Amount = amount;
            Reason = reason?.Trim();
            Type = type;
            
            Status = RefundStatus.Pending;
        }

        public void Approve(Guid approverId)
        {
            if (Status != RefundStatus.Pending)
            {
                throw new InvalidOperationException($"Cannot approve a refund from status {Status}.");
            }

            if (approverId == Guid.Empty) throw new ArgumentException("Approver ID cannot be empty.", nameof(approverId));

            Status = RefundStatus.Approved;
            ApprovedByUserId = approverId;
            ApprovedAtUtc = DateTime.UtcNow;

            AddDomainEvent(new RefundApprovedEvent(Id));
        }

        public void Reject()
        {
            if (Status != RefundStatus.Pending)
            {
                throw new InvalidOperationException($"Cannot reject a refund from status {Status}.");
            }

            Status = RefundStatus.Rejected;
        }

        public void MarkAsProcessing()
        {
            if (Status != RefundStatus.Approved)
            {
                throw new InvalidOperationException($"Cannot process a refund from status {Status}. Only approved refunds can be processed.");
            }

            Status = RefundStatus.Processing;
        }

        public void MarkAsCompleted(string transactionId)
        {
            if (Status == RefundStatus.Completed) return; // Idempotent
            
            if (string.IsNullOrWhiteSpace(transactionId))
                throw new ArgumentException("TransactionId is required to complete a refund.", nameof(transactionId));

            Status = RefundStatus.Completed;
            TransactionId = transactionId.Trim();
            RefundedAtUtc = DateTime.UtcNow;

            AddDomainEvent(new RefundCompletedEvent(Id));
        }

        public void MarkAsFailed(string? reason = null)
        {
            if (Status == RefundStatus.Failed) return; // Idempotent

            Status = RefundStatus.Failed;
            // Optionally could store failure reason on refund if added later
        }
    }
}
