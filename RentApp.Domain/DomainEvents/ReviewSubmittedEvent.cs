using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ReviewSubmittedEvent : DomainEvent
    {
        public Guid ReviewId { get; }
        public Guid BookingId { get; }

        public ReviewSubmittedEvent(Guid reviewId, Guid bookingId)
        {
            ReviewId = reviewId;
            BookingId = bookingId;
        }
    }
}
