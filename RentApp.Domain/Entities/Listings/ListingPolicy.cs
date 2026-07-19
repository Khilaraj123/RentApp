using System;
using RentApp.Domain.Common;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.Entities.Listings
{
    public class ListingPolicy : BaseEntity
    {
        public Guid ListingId { get; private set; }
        public CancellationPolicy CancellationPolicy { get; private set; } = null!;
        public Money? SecurityDeposit { get; private set; }
        public string? DamagePolicy { get; private set; }
        public Money? LateFee { get; private set; }
        public string? ReturnInstructions { get; private set; }

        private ListingPolicy() { } // EF Core

        public ListingPolicy(
            Guid listingId, 
            CancellationPolicy cancellationPolicy, 
            Money? securityDeposit, 
            string? damagePolicy, 
            Money? lateFee, 
            string? returnInstructions)
        {
            ListingId = listingId;
            CancellationPolicy = cancellationPolicy ?? throw new ArgumentNullException(nameof(cancellationPolicy));
            SecurityDeposit = securityDeposit;
            DamagePolicy = damagePolicy;
            LateFee = lateFee;
            ReturnInstructions = returnInstructions;
        }

        public void UpdatePolicies(
            CancellationPolicy cancellationPolicy, 
            Money? securityDeposit, 
            string? damagePolicy, 
            Money? lateFee, 
            string? returnInstructions)
        {
            CancellationPolicy = cancellationPolicy ?? throw new ArgumentNullException(nameof(cancellationPolicy));
            SecurityDeposit = securityDeposit;
            DamagePolicy = damagePolicy;
            LateFee = lateFee;
            ReturnInstructions = returnInstructions;
        }
    }
}
