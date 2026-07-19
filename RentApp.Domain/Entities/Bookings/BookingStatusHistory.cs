using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.Entities.Bookings
{
    public class BookingStatusHistory : BaseEntity
    {
        public Guid BookingId { get; private set; }
        public BookingStatus OldStatus { get; private set; }
        public BookingStatus NewStatus { get; private set; }
        public DateTime ChangedAtUtc { get; private set; }
        public Guid? ChangedByUserId { get; private set; }
        public string? Note { get; private set; }

        public virtual Booking? Booking { get; private set; }

        private BookingStatusHistory() { } // EF Core

        public BookingStatusHistory(
            Guid bookingId, 
            BookingStatus oldStatus, 
            BookingStatus newStatus, 
            string? note = null, 
            Guid? changedByUserId = null)
        {
            BookingId = bookingId;
            OldStatus = oldStatus;
            NewStatus = newStatus;
            Note = note;
            ChangedByUserId = changedByUserId;
            ChangedAtUtc = DateTime.UtcNow;
        }
    }
}
