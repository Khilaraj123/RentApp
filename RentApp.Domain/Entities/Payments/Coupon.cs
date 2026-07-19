using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Payments
{
    public class Coupon : SoftDeleteEntity
    {
        public string Code { get; private set; } = string.Empty;
        
        public CouponType Type { get; private set; }
        public decimal DiscountValue { get; private set; }
        
        public Money? MinimumBookingAmount { get; private set; }
        
        public int UsageLimit { get; private set; }
        public int UsedCount { get; private set; }
        public int MaxUsesPerUser { get; private set; }
        
        public DateTime? StartDate { get; private set; }
        public DateTime? ExpiryDate { get; private set; }
        
        public bool IsActive { get; private set; }

        private Coupon() { } // EF Core

        public Coupon(
            string code, 
            CouponType type, 
            decimal discountValue, 
            int usageLimit, 
            int maxUsesPerUser, 
            Money? minimumBookingAmount = null, 
            DateTime? startDate = null, 
            DateTime? expiryDate = null)
        {
            if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Coupon code cannot be empty.", nameof(code));
            if (discountValue <= 0) throw new ArgumentException("Discount value must be greater than zero.", nameof(discountValue));
            if (usageLimit < 0) throw new ArgumentException("Usage limit cannot be negative.", nameof(usageLimit));
            if (maxUsesPerUser <= 0) throw new ArgumentException("Max uses per user must be greater than zero.", nameof(maxUsesPerUser));
            if (startDate.HasValue && expiryDate.HasValue && startDate.Value > expiryDate.Value)
            {
                throw new ArgumentException("Start date cannot be after expiry date.");
            }

            Code = code.Trim().ToUpperInvariant();
            Type = type;
            DiscountValue = discountValue;
            UsageLimit = usageLimit;
            UsedCount = 0;
            MaxUsesPerUser = maxUsesPerUser;
            MinimumBookingAmount = minimumBookingAmount;
            StartDate = startDate;
            ExpiryDate = expiryDate;
            IsActive = true;
        }

        public void Deactivate()
        {
            IsActive = false;
        }
        
        public void Activate()
        {
            IsActive = true;
        }

        public bool IsValid()
        {
            if (!IsActive) return false;
            
            var now = DateTime.UtcNow;
            if (StartDate.HasValue && now < StartDate.Value) return false;
            if (ExpiryDate.HasValue && now > ExpiryDate.Value) return false;
            if (UsageLimit > 0 && UsedCount >= UsageLimit) return false;
            
            return true;
        }

        public bool CanApply(Money bookingAmount, int currentUsesByUser)
        {
            if (!IsValid()) return false;
            
            if (currentUsesByUser >= MaxUsesPerUser) return false;
            
            if (MinimumBookingAmount != null)
            {
                if (bookingAmount.Currency != MinimumBookingAmount.Currency || bookingAmount.Amount < MinimumBookingAmount.Amount)
                {
                    return false;
                }
            }
            
            return true;
        }

        public Money CalculateDiscount(Money bookingAmount)
        {
            if (!CanApply(bookingAmount, 0)) // Note: user uses check should happen at app layer
            {
                return new Money(0, bookingAmount.Currency);
            }

            if (Type == CouponType.Fixed)
            {
                var discount = Math.Min(DiscountValue, bookingAmount.Amount);
                return new Money(discount, bookingAmount.Currency);
            }
            else // Percentage
            {
                var discount = bookingAmount.Amount * (DiscountValue / 100m);
                return new Money(discount, bookingAmount.Currency);
            }
        }

        public void RecordUsage()
        {
            if (!IsValid())
            {
                throw new InvalidOperationException("Cannot record usage for an invalid coupon.");
            }
            
            UsedCount++;
        }
    }
}
