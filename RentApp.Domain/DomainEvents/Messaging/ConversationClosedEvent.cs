using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ConversationClosedEvent : DomainEvent
    {
        public Guid ConversationId { get; }

        public ConversationClosedEvent(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }
}
