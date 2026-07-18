using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Bookings
{
    public class Booking : SoftDeleteEntity
    {
        public Guid RenterId { get; private set; }
        public Guid ListingId { get; private set; }
        public RentalPeriod Period { get; private set; } = null!;
        public BookingStatus Status { get; private set; }
        public Money TotalAmount { get; private set; } = null!;
        public Money? DepositAmount { get; private set; }
        public BookingCancellationReason CancellationReason { get; private set; }
        public string? DeliveryOption { get; private set; } // Pickup, Delivery
        public int Quantity { get; private set; }

        private readonly List<BookingItem> _items = new();
        public IReadOnlyCollection<BookingItem> Items => _items.AsReadOnly();

        private Booking() { } // EF Core

        public Booking(Guid renterId, Guid listingId, RentalPeriod period, Money totalAmount, Money? depositAmount, string? deliveryOption, int quantity)
        {
            RenterId = renterId;
            ListingId = listingId;
            Period = period;
            TotalAmount = totalAmount;
            DepositAmount = depositAmount;
            DeliveryOption = deliveryOption;
            Quantity = quantity;
            Status = BookingStatus.Pending;
            CancellationReason = BookingCancellationReason.None;
        }

        public void Approve()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException("Only pending bookings can be approved.");
            
            Status = BookingStatus.Confirmed;
        }

        public void Reject()
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException("Only pending bookings can be rejected.");
            
            Status = BookingStatus.Cancelled;
            CancellationReason = BookingCancellationReason.HostRequested;
        }
        
        public void AddItem(BookingItem item)
        {
            _items.Add(item);
        }
    }
}
