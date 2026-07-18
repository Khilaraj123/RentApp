using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Reports
{
    public class Report : AuditableEntity
    {
        public Guid ReporterId { get; private set; }
        public Guid TargetId { get; private set; } // Can be UserId or ListingId
        public string TargetType { get; private set; } = string.Empty; // "User", "Listing"
        public string Reason { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public string Status { get; private set; } = string.Empty; // Pending, Reviewed, ActionTaken, Dismissed

        private Report() { } // EF Core

        public Report(Guid reporterId, Guid targetId, string targetType, string reason, string? description)
        {
            ReporterId = reporterId;
            TargetId = targetId;
            TargetType = targetType;
            Reason = reason;
            Description = description;
            Status = "Pending";
        }

        public void MarkAsReviewed()
        {
            Status = "Reviewed";
        }

        public void TakeAction()
        {
            Status = "ActionTaken";
        }

        public void Dismiss()
        {
            Status = "Dismissed";
        }
    }
}
