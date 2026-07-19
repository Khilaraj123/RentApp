using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.DomainEvents;
using RentApp.Domain.Entities.Users;

namespace RentApp.Domain.Entities.Notifications
{
    public class Notification : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public virtual ApplicationUser? User { get; private set; }

        public NotificationType Type { get; private set; }
        public NotificationChannel Channel { get; private set; }
        
        public NotificationDeliveryStatus DeliveryStatus { get; private set; }
        
        public bool IsRead { get; private set; }
        public DateTime? ReadAtUtc { get; private set; }
        public DateTime? SentAtUtc { get; private set; }
        public DateTime? DeliveredAtUtc { get; private set; }

        public string Title { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;

        public Guid? ReferenceId { get; private set; }
        public NotificationReferenceType? ReferenceType { get; private set; }
        public string? ActionUrl { get; private set; }

        public NotificationPriority Priority { get; private set; }
        public int RetryCount { get; private set; }
        public string? FailureReason { get; private set; }
        public DateTime? ExpiresAtUtc { get; private set; }

        public string? TemplateName { get; private set; }
        public string? TemplateData { get; private set; }

        private Notification() { } // EF Core

        public Notification(
            Guid userId, 
            NotificationType type, 
            NotificationChannel channel, 
            string title, 
            string content,
            NotificationPriority priority = NotificationPriority.Normal)
        {
            if (userId == Guid.Empty) throw new ArgumentException("UserId cannot be empty.", nameof(userId));
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title cannot be empty.", nameof(title));
            if (string.IsNullOrWhiteSpace(content)) throw new ArgumentException("Content cannot be empty.", nameof(content));

            UserId = userId;
            Type = type;
            Channel = channel;
            Title = title.Trim();
            Content = content.Trim();
            Priority = priority;
            
            DeliveryStatus = NotificationDeliveryStatus.Pending;
            IsRead = false;
            RetryCount = 0;

            AddDomainEvent(new NotificationCreatedEvent(Id));
        }

        public void SetReference(Guid referenceId, NotificationReferenceType referenceType, string? actionUrl = null)
        {
            if (referenceId == Guid.Empty) throw new ArgumentException("ReferenceId cannot be empty.", nameof(referenceId));
            
            ReferenceId = referenceId;
            ReferenceType = referenceType;
            ActionUrl = actionUrl;
        }

        public void SetTemplate(string templateName, string? templateData = null)
        {
            if (string.IsNullOrWhiteSpace(templateName)) throw new ArgumentException("TemplateName cannot be empty.", nameof(templateName));
            
            TemplateName = templateName.Trim();
            TemplateData = templateData;
        }

        public void SetExpiration(DateTime expiresAtUtc)
        {
            ExpiresAtUtc = expiresAtUtc;
        }

        public void MarkAsSent()
        {
            if (DeliveryStatus == NotificationDeliveryStatus.Sent || DeliveryStatus == NotificationDeliveryStatus.Delivered)
            {
                return; // Idempotent
            }

            DeliveryStatus = NotificationDeliveryStatus.Sent;
            SentAtUtc = DateTime.UtcNow;

            AddDomainEvent(new NotificationSentEvent(Id));
        }

        public void MarkAsDelivered()
        {
            if (DeliveryStatus == NotificationDeliveryStatus.Delivered)
            {
                return; // Idempotent
            }

            DeliveryStatus = NotificationDeliveryStatus.Delivered;
            DeliveredAtUtc = DateTime.UtcNow;
            
            if (!SentAtUtc.HasValue)
            {
                SentAtUtc = DeliveredAtUtc; // Infer if marked directly
            }
        }

        public void MarkAsFailed(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Failure reason cannot be empty.", nameof(reason));

            DeliveryStatus = NotificationDeliveryStatus.Failed;
            FailureReason = reason.Trim();
            RetryCount++;

            AddDomainEvent(new NotificationFailedEvent(Id));
        }

        public void MarkAsRead()
        {
            if (IsRead)
            {
                return; // Idempotent
            }

            IsRead = true;
            ReadAtUtc = DateTime.UtcNow;
            
            // Note: We don't mark DeliveryStatus as Delivered here because IsRead is an in-app concept,
            // but you could argue if it's read, it must have been delivered to the app.

            AddDomainEvent(new NotificationReadEvent(Id));
        }
    }
}
