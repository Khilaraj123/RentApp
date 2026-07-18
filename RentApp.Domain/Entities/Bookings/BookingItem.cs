using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Bookings
{
    public class BookingItem : BaseEntity
    {
        public Guid BookingId { get; private set; }
        public Guid ListingId { get; private set; }
        public int Quantity { get; private set; }
        public Money UnitPrice { get; private set; } = null!;

        private BookingItem() { } // EF Core

        public BookingItem(Guid bookingId, Guid listingId, int quantity, Money unitPrice)
        {
            BookingId = bookingId;
            ListingId = listingId;
            Quantity = quantity;
            UnitPrice = unitPrice;
        }
    }
}
