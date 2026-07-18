using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Users
{
    public class VerificationRequest : AuditableEntity
    {
        public Guid UserId { get; private set; }
        public string Status { get; private set; } = string.Empty;
        public string? Notes { get; private set; }

        private VerificationRequest() { } // EF Core

        public VerificationRequest(Guid userId)
        {
            UserId = userId;
            Status = "Pending";
        }

        public void Approve(string? notes)
        {
            Status = "Approved";
            Notes = notes;
        }

        public void Reject(string? notes)
        {
            Status = "Rejected";
            Notes = notes;
        }
    }
}
