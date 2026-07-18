using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Messaging
{
    public class Message : BaseEntity
    {
        public Guid ConversationId { get; private set; }
        public Guid SenderId { get; private set; }
        public string Content { get; private set; } = string.Empty;
        public DateTime SentAt { get; private set; }
        public bool IsRead { get; private set; }
        public string? AttachmentUrl { get; private set; }

        private Message() { } // EF Core

        public Message(Guid conversationId, Guid senderId, string content, string? attachmentUrl = null)
        {
            ConversationId = conversationId;
            SenderId = senderId;
            Content = content;
            AttachmentUrl = attachmentUrl;
            SentAt = DateTime.UtcNow;
            IsRead = false;
        }

        public void MarkAsRead()
        {
            IsRead = true;
        }
    }
}
