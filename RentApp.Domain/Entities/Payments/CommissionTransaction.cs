using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class CommissionTransaction : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public Money Amount { get; private set; } = null!;
        public decimal Rate { get; private set; }

        private CommissionTransaction() { } // EF Core

        public CommissionTransaction(Guid bookingId, Money amount, decimal rate)
        {
            BookingId = bookingId;
            Amount = amount;
            Rate = rate;
        }
    }
}
