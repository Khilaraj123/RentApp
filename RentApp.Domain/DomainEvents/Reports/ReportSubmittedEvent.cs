using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ReportSubmittedEvent : DomainEvent
    {
        public Guid ReportId { get; }

        public ReportSubmittedEvent(Guid reportId)
        {
            ReportId = reportId;
        }
    }
}
