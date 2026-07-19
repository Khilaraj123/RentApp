using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class CommissionTransaction : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public Guid PaymentId { get; private set; }
        public Guid OwnerId { get; private set; }
        
        public CommissionStatus Status { get; private set; }
        public CommissionFeeType FeeType { get; private set; }
        
        public Money Amount { get; private set; } = null!;
        public decimal Rate { get; private set; }
        
        public DateTime? SettledAtUtc { get; private set; }

        private CommissionTransaction() { } // EF Core

        public CommissionTransaction(Guid bookingId, Guid paymentId, Guid ownerId, Money amount, decimal rate, CommissionFeeType feeType)
        {
            if (bookingId == Guid.Empty) throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
            if (paymentId == Guid.Empty) throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
            if (ownerId == Guid.Empty) throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));
            if (amount == null) throw new ArgumentNullException(nameof(amount));

            BookingId = bookingId;
            PaymentId = paymentId;
            OwnerId = ownerId;
            Amount = amount;
            Rate = rate;
            FeeType = feeType;
            
            Status = CommissionStatus.Pending;
        }

        public void MarkAsSettled()
        {
            if (Status == CommissionStatus.Settled || Status == CommissionStatus.PaidOut)
            {
                return; // Idempotent
            }
            
            Status = CommissionStatus.Settled;
            SettledAtUtc = DateTime.UtcNow;
        }
        
        public void MarkAsPaidOut()
        {
            if (Status == CommissionStatus.PaidOut) return;
            if (Status != CommissionStatus.Settled)
            {
                throw new InvalidOperationException("Cannot pay out a commission that is not settled.");
            }
            
            Status = CommissionStatus.PaidOut;
        }
    }
}
