using System;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Reports
{
    public class Report : AuditableEntity
    {
        public Guid ReporterId { get; private set; }
        public Guid TargetId { get; private set; }
        
        public ReportTargetType TargetType { get; private set; }
        public ReportReason Reason { get; private set; }
        public string? Description { get; private set; }
        
        public ReportStatus Status { get; private set; }
        
        public Guid? AssignedModeratorId { get; private set; }
        public Guid? ReviewedByUserId { get; private set; }
        public DateTime? ReviewedAtUtc { get; private set; }
        
        public ModerationAction? ActionTaken { get; private set; }
        public string? ResolutionNotes { get; private set; }

        private readonly List<ReportEvidence> _evidence = new();
        public IReadOnlyCollection<ReportEvidence> Evidence => _evidence.AsReadOnly();

        private Report() { } // EF Core

        public Report(Guid reporterId, Guid targetId, ReportTargetType targetType, ReportReason reason, string? description)
        {
            if (reporterId == Guid.Empty) throw new ArgumentException("ReporterId cannot be empty.", nameof(reporterId));
            if (targetId == Guid.Empty) throw new ArgumentException("TargetId cannot be empty.", nameof(targetId));

            if (reporterId == targetId && targetType == ReportTargetType.User)
            {
                throw new ArgumentException("A user cannot report themselves.");
            }

            ReporterId = reporterId;
            TargetId = targetId;
            TargetType = targetType;
            Reason = reason;
            Description = description?.Trim();
            
            Status = ReportStatus.Pending;

            AddDomainEvent(new ReportSubmittedEvent(Id));
        }

        public void AddEvidence(EvidenceFileType type, string evidenceUrl, string? fileName = null, string? contentType = null, long fileSize = 0, string? description = null)
        {
            if (Status != ReportStatus.Pending && Status != ReportStatus.UnderReview)
            {
                throw new InvalidOperationException("Cannot add evidence to a report that has already been resolved.");
            }
            
            if (_evidence.Count >= 10)
            {
                throw new InvalidOperationException("Cannot attach more than 10 evidence files to a single report.");
            }

            var evidence = new ReportEvidence(Id, type, evidenceUrl, fileName, contentType, fileSize, description);
            _evidence.Add(evidence);
        }

        public void RemoveEvidence(Guid evidenceId)
        {
            if (Status != ReportStatus.Pending && Status != ReportStatus.UnderReview)
            {
                throw new InvalidOperationException("Cannot remove evidence from a report that has already been resolved.");
            }

            _evidence.RemoveAll(e => e.Id == evidenceId);
        }

        public void AssignModerator(Guid moderatorId)
        {
            if (moderatorId == Guid.Empty) throw new ArgumentException("ModeratorId cannot be empty.", nameof(moderatorId));
            if (Status == ReportStatus.Closed || Status == ReportStatus.Dismissed || Status == ReportStatus.ActionTaken)
            {
                throw new InvalidOperationException("Cannot assign a moderator to a resolved report.");
            }

            AssignedModeratorId = moderatorId;
            AddDomainEvent(new ReportAssignedEvent(Id, moderatorId));
        }

        public void StartReview(Guid reviewerId)
        {
            if (Status != ReportStatus.Pending) return;

            Status = ReportStatus.UnderReview;
            ReviewedByUserId = reviewerId;
            // Optionally, assign the moderator if not already assigned
            if (!AssignedModeratorId.HasValue)
            {
                AssignedModeratorId = reviewerId;
            }
        }

        public void TakeAction(ModerationAction action, string notes, Guid reviewerId)
        {
            if (Status == ReportStatus.Closed || Status == ReportStatus.ActionTaken || Status == ReportStatus.Dismissed)
            {
                throw new InvalidOperationException("Report is already resolved.");
            }

            if (string.IsNullOrWhiteSpace(notes)) throw new ArgumentException("Resolution notes must be provided when taking action.", nameof(notes));

            Status = ReportStatus.ActionTaken;
            ActionTaken = action;
            ResolutionNotes = notes.Trim();
            ReviewedByUserId = reviewerId;
            ReviewedAtUtc = DateTime.UtcNow;

            AddDomainEvent(new ReportResolvedEvent(Id, Status));
        }

        public void Dismiss(string notes, Guid reviewerId)
        {
            if (Status == ReportStatus.Closed || Status == ReportStatus.ActionTaken || Status == ReportStatus.Dismissed)
            {
                throw new InvalidOperationException("Report is already resolved.");
            }

            Status = ReportStatus.Dismissed;
            ResolutionNotes = notes?.Trim();
            ReviewedByUserId = reviewerId;
            ReviewedAtUtc = DateTime.UtcNow;

            AddDomainEvent(new ReportResolvedEvent(Id, Status));
        }

        public void Close()
        {
            if (Status == ReportStatus.Closed) return;

            Status = ReportStatus.Closed;
            
            // Just marking the report as closed for archiving
            AddDomainEvent(new ReportResolvedEvent(Id, Status));
        }
    }
}
