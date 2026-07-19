using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;

namespace RentApp.Domain.DomainEvents
{
    public class ReportResolvedEvent : DomainEvent
    {
        public Guid ReportId { get; }
        public ReportStatus Status { get; }

        public ReportResolvedEvent(Guid reportId, ReportStatus status)
        {
            ReportId = reportId;
            Status = status;
        }
    }
}
