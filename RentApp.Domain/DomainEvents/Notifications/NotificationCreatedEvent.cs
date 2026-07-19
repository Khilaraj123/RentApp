using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class NotificationCreatedEvent : DomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationCreatedEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
