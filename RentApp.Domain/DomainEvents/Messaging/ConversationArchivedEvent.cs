using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ConversationArchivedEvent : DomainEvent
    {
        public Guid ConversationId { get; }

        public ConversationArchivedEvent(Guid conversationId)
        {
            ConversationId = conversationId;
        }
    }
}
