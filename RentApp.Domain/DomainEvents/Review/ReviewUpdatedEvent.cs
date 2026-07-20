using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Review
{
    public class ReviewUpdatedEvent : DomainEvent
    {
        public Guid ReviewId { get; }

        public ReviewUpdatedEvent(Guid reviewId)
        {
            ReviewId = reviewId;
        }
    }
}
