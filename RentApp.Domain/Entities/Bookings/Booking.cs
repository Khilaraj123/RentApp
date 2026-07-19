using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Bookings
{
    public class Booking : SoftDeleteEntity
    {
        public string BookingNumber { get; private set; } = string.Empty;
        public Guid RenterId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid ListingId { get; private set; }
        public RentalPeriod Period { get; private set; } = null!;
        public int Quantity { get; private set; }
        public Money TotalAmount { get; private set; } = null!;
        public Money? DepositAmount { get; private set; }

        public BookingStatus Status { get; private set; }
        public PaymentStatus PaymentStatus { get; private set; }
        public DeliveryOption? DeliveryOption { get; private set; }
        public BookingCancellationReason CancellationReason { get; private set; }
        
        public bool RequiresOwnerApproval { get; private set; }

        public string? CustomerNote { get; private set; }
        public string? OwnerNote { get; private set; }

        public DateTime? CancelledAtUtc { get; private set; }
        public Guid? CancelledByUserId { get; private set; }
        public DateTime? PickedUpAtUtc { get; private set; }
        public DateTime? ReturnedAtUtc { get; private set; }

        private readonly List<BookingItem> _items = new();
        public IReadOnlyCollection<BookingItem> Items => _items.AsReadOnly();

        private readonly List<BookingStatusHistory> _statusHistory = new();
        public IReadOnlyCollection<BookingStatusHistory> StatusHistory => _statusHistory.AsReadOnly();

        private Booking() { } // EF Core

        public Booking(
            Guid renterId, 
            Guid ownerId,
            Guid listingId, 
            RentalPeriod period, 
            Money totalAmount, 
            Money? depositAmount, 
            DeliveryOption? deliveryOption, 
            int quantity,
            bool requiresOwnerApproval)
        {
            if (quantity <= 0)
                throw new ArgumentException("Quantity must be greater than zero.");
                
            if (totalAmount.Amount <= 0)
                throw new ArgumentException("Total amount must be greater than zero.");

            BookingNumber = GenerateBookingNumber();
            RenterId = renterId;
            OwnerId = ownerId;
            ListingId = listingId;
            Period = period;
            TotalAmount = totalAmount;
            DepositAmount = depositAmount;
            DeliveryOption = deliveryOption;
            Quantity = quantity;
            RequiresOwnerApproval = requiresOwnerApproval;

            Status = BookingStatus.Pending;
            PaymentStatus = PaymentStatus.Pending;
            CancellationReason = BookingCancellationReason.None;
        }

        private static string GenerateBookingNumber()
        {
            return $"BK-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(100000, 999999)}";
        }

        private void ChangeStatus(BookingStatus newStatus, string? note = null, Guid? changedByUserId = null)
        {
            var oldStatus = Status;
            Status = newStatus;
            
            _statusHistory.Add(new BookingStatusHistory(Id, oldStatus, newStatus, note, changedByUserId));
        }

        public void Approve(string? note = null)
        {
            if (Status != BookingStatus.Pending && Status != BookingStatus.WaitingPayment)
                throw new InvalidOperationException("Only pending or waiting payment bookings can be approved.");

            ChangeStatus(BookingStatus.Confirmed, note ?? "Owner approved booking");
            AddDomainEvent(new BookingConfirmedEvent(Id));
        }

        public void Reject(string? note = null)
        {
            if (Status != BookingStatus.Pending)
                throw new InvalidOperationException("Only pending bookings can be rejected.");

            ChangeStatus(BookingStatus.Rejected, note ?? "Owner rejected booking");
            AddDomainEvent(new BookingRejectedEvent(Id));
        }

        public void Cancel(Guid userId, BookingCancellationReason reason, string? note = null)
        {
            if (Status == BookingStatus.Completed || Status == BookingStatus.Cancelled || Status == BookingStatus.Rejected)
                throw new InvalidOperationException("Booking cannot be cancelled from its current state.");

            ChangeStatus(BookingStatus.Cancelled, note ?? "Booking cancelled");
            CancellationReason = reason;
            CancelledAtUtc = DateTime.UtcNow;
            CancelledByUserId = userId;

            AddDomainEvent(new BookingCancelledEvent(Id));
        }

        public void MarkPaymentReceived()
        {
            if (PaymentStatus == PaymentStatus.Completed)
                return;

            PaymentStatus = PaymentStatus.Completed;
            
            if (!RequiresOwnerApproval && Status == BookingStatus.Pending)
            {
                ChangeStatus(BookingStatus.Confirmed, "Instant booking confirmed after payment");
                AddDomainEvent(new BookingConfirmedEvent(Id));
            }
            else if (Status == BookingStatus.Pending)
            {
                ChangeStatus(BookingStatus.WaitingPayment, "Payment received, waiting for owner approval");
            }
            
            AddDomainEvent(new BookingPaidEvent(Id));
        }

        public void MarkPickedUp()
        {
            if (Status != BookingStatus.Confirmed)
                throw new InvalidOperationException("Only confirmed bookings can be picked up.");

            ChangeStatus(BookingStatus.PickedUp, "Items picked up");
            ChangeStatus(BookingStatus.Active, "Booking is now active");
            PickedUpAtUtc = DateTime.UtcNow;
            
            AddDomainEvent(new ItemPickedUpEvent(Id));
        }

        public void MarkReturned()
        {
            if (Status != BookingStatus.Active)
                throw new InvalidOperationException("Only active bookings can be returned.");

            ChangeStatus(BookingStatus.Returned, "Items returned");
            ReturnedAtUtc = DateTime.UtcNow;
            
            AddDomainEvent(new ItemReturnedEvent(Id));
        }

        public void Complete()
        {
            if (Status != BookingStatus.Returned)
                throw new InvalidOperationException("Only returned bookings can be completed.");

            ChangeStatus(BookingStatus.Completed, "Booking completed successfully");
            
            AddDomainEvent(new BookingCompletedEvent(Id));
        }

        public void Expire()
        {
            if (Status != BookingStatus.Pending && Status != BookingStatus.WaitingPayment)
                throw new InvalidOperationException("Only pending or waiting payment bookings can expire.");

            ChangeStatus(BookingStatus.Expired, "Booking request expired");
            
            AddDomainEvent(new BookingExpiredEvent(Id));
        }
        
        public void RequestRefund()
        {
            if (PaymentStatus != PaymentStatus.Completed)
                throw new InvalidOperationException("Cannot request refund if payment is not completed.");
                
            PaymentStatus = PaymentStatus.Refunded;
        }

        public void AddItem(BookingItem item)
        {
            _items.Add(item);
        }

        public void SetCustomerNote(string note)
        {
            CustomerNote = note;
        }

        public void SetOwnerNote(string note)
        {
            OwnerNote = note;
        }
    }
}
