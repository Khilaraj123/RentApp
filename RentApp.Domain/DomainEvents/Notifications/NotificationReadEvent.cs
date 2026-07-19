using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class NotificationReadEvent : DomainEvent
    {
        public Guid NotificationId { get; }

        public NotificationReadEvent(Guid notificationId)
        {
            NotificationId = notificationId;
        }
    }
}
