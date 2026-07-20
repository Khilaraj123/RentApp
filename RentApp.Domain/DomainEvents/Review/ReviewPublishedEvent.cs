using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Review
{
    public class ReviewPublishedEvent : DomainEvent
    {
        public Guid ReviewId { get; }

        public ReviewPublishedEvent(Guid reviewId)
        {
            ReviewId = reviewId;
        }
    }
}
