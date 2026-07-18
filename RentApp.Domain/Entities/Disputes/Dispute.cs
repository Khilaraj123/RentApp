using System;
using System.Collections.Generic;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Disputes
{
    public class Dispute : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public Guid InitiatorId { get; private set; }
        public string Reason { get; private set; } = string.Empty;
        public string Status { get; private set; } = string.Empty; // Pending, Investigating, Resolved, Closed
        public string? Resolution { get; private set; }

        private readonly List<Evidence> _evidence = new();
        public IReadOnlyCollection<Evidence> Evidence => _evidence.AsReadOnly();

        private Dispute() { } // EF Core

        public Dispute(Guid bookingId, Guid initiatorId, string reason)
        {
            BookingId = bookingId;
            InitiatorId = initiatorId;
            Reason = reason;
            Status = "Pending";
        }

        public void StartInvestigation()
        {
            Status = "Investigating";
        }

        public void Resolve(string resolution)
        {
            Status = "Resolved";
            Resolution = resolution;
        }

        public void AddEvidence(Evidence evidence)
        {
            _evidence.Add(evidence);
        }
    }
}
