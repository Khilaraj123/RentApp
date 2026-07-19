using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class NotificationSentEvent : DomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationSentEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
