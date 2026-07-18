using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Users
{
    public class IdentityDocument : BaseEntity
    {
        public Guid UserId { get; private set; }
        public string DocumentType { get; private set; } = string.Empty;
        public string DocumentUrl { get; private set; } = string.Empty;
        public string VerificationStatus { get; private set; } = string.Empty;
        public DateTime UploadedAt { get; private set; }

        private IdentityDocument() { } // EF Core

        public IdentityDocument(Guid userId, string documentType, string documentUrl)
        {
            UserId = userId;
            DocumentType = documentType;
            DocumentUrl = documentUrl;
            VerificationStatus = "Pending";
            UploadedAt = DateTime.UtcNow;
        }

        public void Approve()
        {
            VerificationStatus = "Approved";
        }

        public void Reject()
        {
            VerificationStatus = "Rejected";
        }
    }
}
