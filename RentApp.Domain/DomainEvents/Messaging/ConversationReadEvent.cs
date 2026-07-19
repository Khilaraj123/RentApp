using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ConversationReadEvent : DomainEvent
    {
        public Guid ConversationId { get; }
        public Guid UserId { get; }

        public ConversationReadEvent(Guid conversationId, Guid userId)
        {
            ConversationId = conversationId;
            UserId = userId;
        }
    }
}
