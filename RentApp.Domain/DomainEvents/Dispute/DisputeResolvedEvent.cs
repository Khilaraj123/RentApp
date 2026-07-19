using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class DisputeResolvedEvent : DomainEvent
    {
        public Guid DisputeId { get; }

        public DisputeResolvedEvent(Guid disputeId)
        {
            DisputeId = disputeId;
        }
    }
}
