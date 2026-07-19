using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class MessageSentEvent : DomainEvent
    {
        public Guid ConversationId { get; }
        public Guid MessageId { get; }

        public MessageSentEvent(Guid conversationId, Guid messageId)
        {
            ConversationId = conversationId;
            MessageId = messageId;
        }
    }
}
