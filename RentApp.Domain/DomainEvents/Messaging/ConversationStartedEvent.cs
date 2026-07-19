using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ConversationStartedEvent : DomainEvent
    {
        public Guid ConversationId { get; }

        public ConversationStartedEvent(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }
}
