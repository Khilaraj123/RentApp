using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents.Review
{
    public class ReviewDeletedEvent : DomainEvent
    {
        public Guid ReviewId { get; }

        public ReviewDeletedEvent(Guid reviewId)
        {
            ReviewId = reviewId;
        }
    }
}
