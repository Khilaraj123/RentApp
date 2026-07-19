using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class NotificationFailedEvent : DomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationFailedEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
