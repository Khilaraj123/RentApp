using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class AgreementFullySignedEvent : DomainEvent
    {
        public Guid AgreementId { get; }

        public AgreementFullySignedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }
    }
}
