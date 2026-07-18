using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.Entities.Notifications
{
    public class Notification : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public NotificationType Type { get; private set; }
        public NotificationChannel Channel { get; private set; }
        public NotificationStatus Status { get; private set; }
        public string Title { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;
        public DateTime? ReadAt { get; private set; }

        private Notification() { } // EF Core

        public Notification(Guid userId, NotificationType type, NotificationChannel channel, string title, string content)
        {
            UserId = userId;
            Type = type;
            Channel = channel;
            Title = title;
            Content = content;
            Status = NotificationStatus.Pending;
        }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
        }
        
        public void MarkAsFailed()
        {
            Status = NotificationStatus.Failed;
        }

        public void MarkAsRead()
        {
            Status = NotificationStatus.Read;
            ReadAt = DateTime.UtcNow;
        }
    }
}
