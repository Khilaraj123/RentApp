using System;
using System.Linq;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.Entities.Messaging
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; private set; }
        public Guid SenderId { get; private set; }
        
        public MessageType Type { get; private set; }
        public string Content { get; private set; } = string.Empty;
        
        public DateTime SentAt { get; private set; }

        private readonly List<MessageAttachment> _attachments = new();
        public IReadOnlyCollection<MessageAttachment> Attachments => _attachments.AsReadOnly();

        private Message() { } // EF Core

        public Message(Guid conversationId, Guid senderId, MessageType type, string? content = null)
        {
            if (conversationId == Guid.Empty) throw new ArgumentException("ConversationId cannot be empty.", nameof(conversationId));
            if (senderId == Guid.Empty) throw new ArgumentException("SenderId cannot be empty.", nameof(senderId));

            if (type == MessageType.Text && string.IsNullOrWhiteSpace(content))
            {
                throw new ArgumentException("Text messages must have content.", nameof(content));
            }

            ConversationId = conversationId;
            SenderId = senderId;
            Type = type;
            Content = content?.Trim() ?? string.Empty;
            SentAt = DateTime.UtcNow;
        }

        public void AddAttachment(MessageAttachment attachment)
        {
            if (attachment == null) throw new ArgumentNullException(nameof(attachment));
            
            if (_attachments.Any(x => x.Id == attachment.Id)) return;

            _attachments.Add(attachment);
        }
    }
}
