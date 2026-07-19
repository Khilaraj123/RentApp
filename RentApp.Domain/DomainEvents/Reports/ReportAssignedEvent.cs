using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class ReportAssignedEvent : DomainEvent
    {
        public Guid ReportId { get; }
        public Guid AssignedModeratorId { get; }

        public ReportAssignedEvent(Guid reportId, Guid assignedModeratorId)
        {
            ReportId = reportId;
            AssignedModeratorId = assignedModeratorId;
        }
    }
}
