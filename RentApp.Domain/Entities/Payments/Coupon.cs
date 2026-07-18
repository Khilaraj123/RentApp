using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class Coupon : SoftDeleteEntity
    {
        public string Code { get; private set; } = string.Empty;
        public decimal? DiscountPercentage { get; private set; }
        public Money? MaxDiscountAmount { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        public bool IsActive { get; private set; }

        private Coupon() { } // EF Core

        public Coupon(string code, decimal? discountPercentage, Money? maxDiscountAmount, DateTime? expiryDate)
        {
            Code = code;
            DiscountPercentage = discountPercentage;
            MaxDiscountAmount = maxDiscountAmount;
            ExpiryDate = expiryDate;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
    }
}
