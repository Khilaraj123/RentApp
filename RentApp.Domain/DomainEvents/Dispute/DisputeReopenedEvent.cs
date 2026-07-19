using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class DisputeReopenedEvent : DomainEvent
    {
        public Guid DisputeId { get; }

        public DisputeReopenedEvent(Guid disputeId)
        {
            DisputeId = disputeId;
        }
    }
}
