using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents.Review;

namespace RentApp.Domain.Entities.Reviews
{
    public class Review : SoftDeleteEntity
    {
        private readonly List<ReviewImage> _images = new();

        public Guid BookingId { get; private set; }
        public Guid ReviewerId { get; private set; }
        public Guid RevieweeId { get; private set; }
        public ReviewTargetType TargetType { get; private set; }

        public Rating OverallRating { get; private set; } = null!;
        public Rating? CommunicationRating { get; private set; }
        public Rating? AccuracyRating { get; private set; }
        public Rating? ConditionRating { get; private set; }
        public Rating? ValueRating { get; private set; }
        public Rating? TimelinessRating { get; private set; }

        public string? Title { get; private set; }
        public string Content { get; private set; } = string.Empty;

        public ReviewStatus Status { get; private set; }
        public string? Response { get; private set; }
        public DateTime? RespondedAtUtc { get; private set; }

        public bool IsVerified { get; private set; }
        public int HelpfulCount { get; private set; }
        public int ReportCount { get; private set; }

        public IReadOnlyCollection<ReviewImage> Images => _images.AsReadOnly();

        private Review() { } // EF Core

        public Review(
            Guid bookingId,
            Guid reviewerId, 
            Guid revieweeId, 
            ReviewTargetType targetType,
            Rating overallRating, 
            string content,
            string? title = null,
            Rating? communicationRating = null, 
            Rating? accuracyRating = null,
            Rating? conditionRating = null,
            Rating? valueRating = null,
            Rating? timelinessRating = null,
            bool isVerified = true)
        {
            if (reviewerId == revieweeId)
                throw new ArgumentException("Reviewer cannot be the same as Reviewee.");

            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Review content cannot be empty.");

            if (content.Length > 2000)
                throw new ArgumentException("Review content is too long.");

            BookingId = bookingId;
            ReviewerId = reviewerId;
            RevieweeId = revieweeId;
            TargetType = targetType;
            
            OverallRating = overallRating;
            Content = content;
            Title = title;
            
            CommunicationRating = communicationRating;
            AccuracyRating = accuracyRating;
            ConditionRating = conditionRating;
            ValueRating = valueRating;
            TimelinessRating = timelinessRating;

            Status = ReviewStatus.Pending;
            IsVerified = isVerified;

            AddDomainEvent(new ReviewCreatedEvent(Id, BookingId));
        }

        public void Respond(string response)
        {
            if (string.IsNullOrWhiteSpace(response))
                throw new ArgumentException("Response cannot be empty.");

            Response = response;
            RespondedAtUtc = DateTime.UtcNow;
        }

        public void Update(
            Rating overallRating, 
            string content,
            string? title = null,
            Rating? communicationRating = null, 
            Rating? accuracyRating = null,
            Rating? conditionRating = null,
            Rating? valueRating = null,
            Rating? timelinessRating = null)
        {
            if (string.IsNullOrWhiteSpace(content))
                throw new ArgumentException("Review content cannot be empty.");

            if (content.Length > 2000)
                throw new ArgumentException("Review content is too long.");

            OverallRating = overallRating;
            Content = content;
            Title = title;
            
            CommunicationRating = communicationRating;
            AccuracyRating = accuracyRating;
            ConditionRating = conditionRating;
            ValueRating = valueRating;
            TimelinessRating = timelinessRating;

            AddDomainEvent(new ReviewUpdatedEvent(Id));
        }

        public void Publish()
        {
            Status = ReviewStatus.Published;
            AddDomainEvent(new ReviewPublishedEvent(Id));
        }

        public void Hide()
        {
            Status = ReviewStatus.Hidden;
        }

        public void Report()
        {
            ReportCount++;
            if (ReportCount >= 5)
            {
                Status = ReviewStatus.Reported;
            }
        }

        public void MarkHelpful()
        {
            HelpfulCount++;
        }
        
        public void AddImage(ReviewImage image)
        {
            _images.Add(image);
        }

        public void Delete()
        {
            IsDeleted = true;
            DeletedAt = DateTime.UtcNow;
            AddDomainEvent(new ReviewDeletedEvent(Id));
        }
    }
}
