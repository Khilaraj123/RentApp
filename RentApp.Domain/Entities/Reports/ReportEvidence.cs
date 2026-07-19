using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.Entities.Reports
{
    public class ReportEvidence : AuditableEntity
    {
        public Guid ReportId { get; private set; }
        
        public EvidenceFileType Type { get; private set; }
        
        public string EvidenceUrl { get; private set; } = string.Empty;
        public string? FileName { get; private set; }
        public string? ContentType { get; private set; }
        public long FileSize { get; private set; }
        
        public string? Description { get; private set; }

        private ReportEvidence() { } // EF Core

        internal ReportEvidence(
            Guid reportId, 
            EvidenceFileType type, 
            string evidenceUrl, 
            string? fileName = null, 
            string? contentType = null, 
            long fileSize = 0, 
            string? description = null)
        {
            if (reportId == Guid.Empty) throw new ArgumentException("ReportId cannot be empty.", nameof(reportId));
            if (string.IsNullOrWhiteSpace(evidenceUrl)) throw new ArgumentException("EvidenceUrl cannot be empty.", nameof(evidenceUrl));

            ReportId = reportId;
            Type = type;
            EvidenceUrl = evidenceUrl.Trim();
            FileName = fileName?.Trim();
            ContentType = contentType?.Trim();
            FileSize = fileSize;
            Description = description?.Trim();
        }
    }
}