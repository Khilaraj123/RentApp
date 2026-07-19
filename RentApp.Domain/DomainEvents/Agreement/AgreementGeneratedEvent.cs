using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.DomainEvents
{
    public class AgreementGeneratedEvent : DomainEvent
    {
        public Guid AgreementId { get; }

        public AgreementGeneratedEvent(Guid agreementId)
        {
            AgreementId = agreementId;
        }
    }
}
