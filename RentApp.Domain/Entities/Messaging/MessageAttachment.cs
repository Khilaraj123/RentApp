using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.Entities.Messaging
{
    public class MessageAttachment : BaseEntity
    {
        public Guid MessageId { get; private set; }

        public string FileName { get; private set; } = string.Empty;

        public string FileUrl { get; private set; } = string.Empty;

        public string ContentType { get; private set; } = string.Empty;

        public long FileSize { get; private set; }

        public AttachmentType Type { get; private set; }

        private MessageAttachment() { } // EF Core

        public MessageAttachment(
            Guid messageId,
            string fileName,
            string fileUrl,
            string contentType,
            long fileSize,
            AttachmentType type)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentException("FileName cannot be empty.", nameof(fileName));
            if (string.IsNullOrWhiteSpace(fileUrl)) throw new ArgumentException("FileUrl cannot be empty.", nameof(fileUrl));
            if (string.IsNullOrWhiteSpace(contentType)) throw new ArgumentException("ContentType cannot be empty.", nameof(contentType));
            if (fileSize < 0) throw new ArgumentException("FileSize cannot be negative.", nameof(fileSize));

            MessageId = messageId;
            FileName = fileName.Trim();
            FileUrl = fileUrl.Trim();
            ContentType = contentType.Trim();
            FileSize = fileSize;
            Type = type;
        }
    }
}