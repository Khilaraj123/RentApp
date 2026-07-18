using System;
using RentApp.Domain.Common;

namespace RentApp.Domain.Entities.Agreements
{
    public class RentalAgreement : AuditableEntity
    {
        public Guid BookingId { get; private set; }
        public string DocumentUrl { get; private set; } = string.Empty;
        public DateTime? SignedByRenterAt { get; private set; }
        public DateTime? SignedByOwnerAt { get; private set; }

        private RentalAgreement() { } // EF Core

        public RentalAgreement(Guid bookingId, string documentUrl)
        {
            BookingId = bookingId;
            DocumentUrl = documentUrl;
        }

        public void SignAsRenter()
        {
            SignedByRenterAt = DateTime.UtcNow;
        }

        public void SignAsOwner()
        {
            SignedByOwnerAt = DateTime.UtcNow;
        }
    }
}
