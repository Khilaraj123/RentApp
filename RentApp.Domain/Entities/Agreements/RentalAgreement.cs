using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.Entities.Agreements
{
    public class RentalAgreement : AuditableEntity
    {
        public string AgreementNumber { get; private set; } = string.Empty;
        
        public Guid BookingId { get; private set; }
        public Guid ListingId { get; private set; }
        public Guid OwnerId { get; private set; }
        public Guid RenterId { get; private set; }
        
        public string ListingTitle { get; private set; } = string.Empty;
        public Money RentalAmount { get; private set; } = null!;
        public Money DepositAmount { get; private set; } = null!;
        public RentalPeriod RentalPeriod { get; private set; } = null!;

        public AgreementStatus Status { get; private set; }

        public string DocumentUrl { get; private set; } = string.Empty;
        public string DocumentFileName { get; private set; } = string.Empty;
        public long DocumentSize { get; private set; }
        public string DocumentHash { get; private set; } = string.Empty;

        public DateTime? SignedByOwnerAt { get; private set; }
        public Guid? SignedByOwnerId { get; private set; }
        public string? OwnerIpAddress { get; private set; }
        public string? OwnerUserAgent { get; private set; }

        public DateTime? SignedByRenterAt { get; private set; }
        public Guid? SignedByRenterId { get; private set; }
        public string? RenterIpAddress { get; private set; }
        public string? RenterUserAgent { get; private set; }

        public DateTime? ExpiresAtUtc { get; private set; }
        
        public DateTime? CancelledAtUtc { get; private set; }
        public string? CancellationReason { get; private set; }

        public string TermsAndConditions { get; private set; } = string.Empty;
        public string DamagePolicy { get; private set; } = string.Empty;
        public string CancellationPolicy { get; private set; } = string.Empty;

        public virtual Booking? Booking { get; private set; }

        private RentalAgreement() { } // EF Core

        public RentalAgreement(
            Guid bookingId, 
            Guid listingId, 
            Guid ownerId, 
            Guid renterId,
            string listingTitle,
            Money rentalAmount,
            Money depositAmount,
            RentalPeriod rentalPeriod,
            string termsAndConditions,
            string damagePolicy,
            string cancellationPolicy)
        {
            if (bookingId == Guid.Empty) throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
            if (listingId == Guid.Empty) throw new ArgumentException("ListingId cannot be empty.", nameof(listingId));
            if (ownerId == Guid.Empty) throw new ArgumentException("OwnerId cannot be empty.", nameof(ownerId));
            if (renterId == Guid.Empty) throw new ArgumentException("RenterId cannot be empty.", nameof(renterId));
            if (string.IsNullOrWhiteSpace(listingTitle)) throw new ArgumentException("ListingTitle cannot be empty.", nameof(listingTitle));

            AgreementNumber = GenerateAgreementNumber();
            
            BookingId = bookingId;
            ListingId = listingId;
            OwnerId = ownerId;
            RenterId = renterId;
            ListingTitle = listingTitle;
            RentalAmount = rentalAmount;
            DepositAmount = depositAmount;
            RentalPeriod = rentalPeriod;
            
            TermsAndConditions = termsAndConditions;
            DamagePolicy = damagePolicy;
            CancellationPolicy = cancellationPolicy;

            Status = AgreementStatus.Draft;
        }

        private static string GenerateAgreementNumber()
        {
            return $"AGR-{DateTime.UtcNow:yyyyMMdd}-{new Random().Next(100000, 999999)}";
        }

        public void Generate(string documentUrl, string documentFileName, long documentSize, string documentHash)
        {
            if (Status != AgreementStatus.Draft && Status != AgreementStatus.Generated)
                throw new InvalidOperationException("Can only generate document for draft agreements or regenerate an existing un-signed one.");
                
            if (string.IsNullOrWhiteSpace(documentUrl)) throw new ArgumentException("DocumentUrl cannot be empty.", nameof(documentUrl));
            if (string.IsNullOrWhiteSpace(documentFileName)) throw new ArgumentException("DocumentFileName cannot be empty.", nameof(documentFileName));
            if (string.IsNullOrWhiteSpace(documentHash)) throw new ArgumentException("DocumentHash cannot be empty.", nameof(documentHash));
            if (documentSize <= 0) throw new ArgumentException("DocumentSize must be greater than zero.", nameof(documentSize));

            DocumentUrl = documentUrl;
            DocumentFileName = documentFileName;
            DocumentSize = documentSize;
            DocumentHash = documentHash;

            Status = AgreementStatus.Generated;
            
            AddDomainEvent(new AgreementGeneratedEvent(Id));
        }
        
        public void ReplaceDocument(string documentUrl, string documentFileName, long documentSize, string documentHash)
        {
            if (SignedByOwnerAt != null || SignedByRenterAt != null)
                throw new InvalidOperationException("Cannot replace document after it has been signed.");
                
            Generate(documentUrl, documentFileName, documentSize, documentHash);
        }

        public void SignByOwner(Guid userId, string? ipAddress, string? userAgent)
        {
            if (Status == AgreementStatus.Cancelled || Status == AgreementStatus.Expired || Status == AgreementStatus.Draft)
                throw new InvalidOperationException("Agreement is not in a valid state to be signed.");

            if (SignedByOwnerAt != null)
                throw new InvalidOperationException("Agreement has already been signed by the owner.");

            SignedByOwnerId = userId;
            SignedByOwnerAt = DateTime.UtcNow;
            OwnerIpAddress = ipAddress;
            OwnerUserAgent = userAgent;

            CheckFullySignedStatus();
        }

        public void SignByRenter(Guid userId, string? ipAddress, string? userAgent)
        {
            if (Status == AgreementStatus.Cancelled || Status == AgreementStatus.Expired || Status == AgreementStatus.Draft)
                throw new InvalidOperationException("Agreement is not in a valid state to be signed.");

            if (SignedByRenterAt != null)
                throw new InvalidOperationException("Agreement has already been signed by the renter.");

            SignedByRenterId = userId;
            SignedByRenterAt = DateTime.UtcNow;
            RenterIpAddress = ipAddress;
            RenterUserAgent = userAgent;

            CheckFullySignedStatus();
        }
        
        private void CheckFullySignedStatus()
        {
            if (SignedByOwnerAt != null && SignedByRenterAt != null)
            {
                Status = AgreementStatus.FullySigned;
                AddDomainEvent(new AgreementFullySignedEvent(Id));
            }
            else if (SignedByOwnerAt != null)
            {
                Status = AgreementStatus.WaitingForRenter;
            }
            else if (SignedByRenterAt != null)
            {
                Status = AgreementStatus.WaitingForOwner;
            }
        }

        public void Cancel(string reason)
        {
            if (Status == AgreementStatus.FullySigned)
                throw new InvalidOperationException("Cannot cancel a fully signed agreement. It must be voided or overridden.");

            Status = AgreementStatus.Cancelled;
            CancelledAtUtc = DateTime.UtcNow;
            CancellationReason = reason;
        }

        public void Expire()
        {
            if (Status == AgreementStatus.FullySigned)
                throw new InvalidOperationException("Fully signed agreements do not expire.");

            Status = AgreementStatus.Expired;
            ExpiresAtUtc = DateTime.UtcNow;
        }
        
        public void MarkCompleted()
        {
            if (Status != AgreementStatus.FullySigned)
                throw new InvalidOperationException("Only fully signed agreements can be marked as completed.");
        }
    }
}
