using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Review
{
    public class ReviewCreatedEvent : DomainEvent
    {
        public Guid ReviewId { get; }
        public Guid BookingId { get; }

        public ReviewCreatedEvent(Guid reviewId, Guid bookingId)
        {
            ReviewId = reviewId;
            BookingId = bookingId;
        }
    }
}
