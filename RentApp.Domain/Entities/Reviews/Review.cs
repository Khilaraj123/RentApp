using System;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Reviews
{
    public class Review : SoftDeleteEntity
    {
        public Guid ReviewerId { get; private set; }
        public Guid RevieweeId { get; private set; }
        public Guid BookingId { get; private set; }
        public Rating OverallRating { get; private set; } = null!;
        public Rating? CommunicationRating { get; private set; }
        public Rating? AccuracyRating { get; private set; }
        public string Content { get; private set; } = string.Empty;

        private Review() { } // EF Core

        public Review(Guid reviewerId, Guid revieweeId, Guid bookingId, Rating overallRating, Rating? communicationRating, Rating? accuracyRating, string content)
        {
            ReviewerId = reviewerId;
            RevieweeId = revieweeId;
            BookingId = bookingId;
            OverallRating = overallRating;
            CommunicationRating = communicationRating;
            AccuracyRating = accuracyRating;
            Content = content;
        }
    }
}
