using System;
using System.Linq;
using System.Collections.Generic;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.DomainEvents;
using RentApp.Domain.Entities.Bookings;

namespace RentApp.Domain.Entities.Disputes
{
    public class Dispute : SoftDeleteEntity
    {
        public Guid BookingId { get; private set; }
        public virtual Booking? Booking { get; private set; }

        public Guid RenterId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid InitiatorId { get; private set; }
        public Guid? AssignedAdminId { get; private set; }

        public Guid? PaymentId { get; private set; }
        public Guid? RefundId { get; private set; }

        public DisputeType Type { get; private set; }
        public DisputeStatus Status { get; private set; }
        public DisputeResolutionType? ResolutionType { get; private set; }

        public string Reason { get; private set; } = string.Empty;
        public string? Resolution { get; private set; }
        public string? InternalNotes { get; private set; }

        public DateTime? ResolvedAtUtc { get; private set; }
        public Guid? ResolvedByUserId { get; private set; }
        public DateTime? ClosedAtUtc { get; private set; }

        private readonly List<Evidence> _evidence = new();
        public IReadOnlyCollection<Evidence> Evidence => _evidence.AsReadOnly();

        private Dispute() { } // EF Core

        public Dispute(
            Guid bookingId, 
            Guid renterId, 
            Guid ownerId, 
            Guid initiatorId, 
            DisputeType type, 
            string reason,
            Guid? paymentId = null,
            Guid? refundId = null)
        {
            if (bookingId == Guid.Empty) throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
            if (renterId == Guid.Empty) throw new ArgumentException("RenterId cannot be empty.", nameof(renterId));
            if (ownerId == Guid.Empty) throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));
            if (initiatorId == Guid.Empty) throw new ArgumentException("InitiatorId cannot be empty.", nameof(initiatorId));
            if (string.IsNullOrWhiteSpace(reason)) throw new ArgumentException("Reason cannot be empty.", nameof(reason));

            BookingId = bookingId;
            RenterId = renterId;
            OwnerId = ownerId;
            InitiatorId = initiatorId;
            Type = type;
            Reason = reason.Trim();
            PaymentId = paymentId;
            RefundId = refundId;

            Status = DisputeStatus.Pending;
        }

        public void AssignAdmin(Guid adminId)
        {
            if (adminId == Guid.Empty) throw new ArgumentException("AdminId cannot be empty.", nameof(adminId));
            AssignedAdminId = adminId;
        }

        public void UpdateInternalNotes(string notes)
        {
            InternalNotes = notes;
        }

        public void StartInvestigation()
        {
            if (Status != DisputeStatus.Pending && Status != DisputeStatus.WaitingForEvidence)
            {
                throw new InvalidOperationException($"Cannot start investigation from status {Status}.");
            }

            Status = DisputeStatus.UnderInvestigation;
        }

        public void RequestMoreEvidence()
        {
            if (Status != DisputeStatus.Pending && Status != DisputeStatus.UnderInvestigation)
            {
                throw new InvalidOperationException($"Cannot request evidence from status {Status}.");
            }

            Status = DisputeStatus.WaitingForEvidence;
        }

        public void Resolve(DisputeResolutionType resolutionType, string resolution, Guid resolvedByUserId)
        {
            if (Status != DisputeStatus.UnderInvestigation && Status != DisputeStatus.WaitingForEvidence)
            {
                throw new InvalidOperationException($"Cannot resolve dispute from status {Status}.");
            }

            if (string.IsNullOrWhiteSpace(resolution))
            {
                throw new ArgumentException("Resolution details cannot be empty.", nameof(resolution));
            }

            if (resolvedByUserId == Guid.Empty)
            {
                throw new ArgumentException("ResolvedByUserId cannot be empty.", nameof(resolvedByUserId));
            }

            Status = DisputeStatus.Resolved;
            ResolutionType = resolutionType;
            Resolution = resolution.Trim();
            ResolvedByUserId = resolvedByUserId;
            ResolvedAtUtc = DateTime.UtcNow;

            AddDomainEvent(new DisputeResolvedEvent(Id));
        }

        public void Reject(string reason)
        {
            if (Status == DisputeStatus.Resolved || Status == DisputeStatus.Closed)
            {
                throw new InvalidOperationException($"Cannot reject a dispute that is already {Status}.");
            }
            
            if (string.IsNullOrWhiteSpace(reason))
            {
                throw new ArgumentException("Rejection reason cannot be empty.", nameof(reason));
            }

            Status = DisputeStatus.Rejected;
            Resolution = reason.Trim();
            ResolvedAtUtc = DateTime.UtcNow;
        }

        public void Close()
        {
            if (Status != DisputeStatus.Resolved && Status != DisputeStatus.Rejected)
            {
                throw new InvalidOperationException($"Cannot close dispute from status {Status}. Must be Resolved or Rejected first.");
            }

            Status = DisputeStatus.Closed;
            ClosedAtUtc = DateTime.UtcNow;
        }

        public void Reopen()
        {
            if (Status != DisputeStatus.Resolved && Status != DisputeStatus.Rejected && Status != DisputeStatus.Closed)
            {
                throw new InvalidOperationException($"Cannot reopen a dispute that is {Status}.");
            }

            Status = DisputeStatus.UnderInvestigation;
            ClosedAtUtc = null;
            ResolvedAtUtc = null;
            ResolvedByUserId = null;
            Resolution = null;
            ResolutionType = null;

            AddDomainEvent(new DisputeReopenedEvent(Id));
        }

        public void AddEvidence(Evidence evidence)
        {
            if (evidence == null) throw new ArgumentNullException(nameof(evidence));

            if (_evidence.Any(x => x.Id == evidence.Id))
                return;

            _evidence.Add(evidence);
        }

        public void RemoveEvidence(Guid evidenceId)
        {
            var evidence = _evidence.FirstOrDefault(x => x.Id == evidenceId);
            if (evidence != null)
            {
                _evidence.Remove(evidence);
            }
        }
    }
}
