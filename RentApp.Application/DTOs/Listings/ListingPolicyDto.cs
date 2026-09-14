using System;

namespace RentApp.Application.DTOs.Listings;

public class ListingPolicyDto
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string CancellationPolicyName { get; set; } = string.Empty;
    public string CancellationPolicyDescription { get; set; } = string.Empty;
    public decimal? SecurityDepositAmount { get; set; }
    public string? SecurityDepositCurrency { get; set; }
    public string? DamagePolicy { get; set; }
    public decimal? LateFeeAmount { get; set; }
    public string? LateFeeCurrency { get; set; }
    public string? ReturnInstructions { get; set; }
}

public class CreateListingPolicyDto
{
    public string CancellationPolicyName { get; set; } = "Flexible";
    public string? CancellationPolicyDescription { get; set; }
    public decimal? SecurityDepositAmount { get; set; }
    public string? SecurityDepositCurrency { get; set; } = "USD";
    public string? DamagePolicy { get; set; }
    public decimal? LateFeeAmount { get; set; }
    public string? LateFeeCurrency { get; set; } = "USD";
    public string? ReturnInstructions { get; set; }
}

public class UpdateListingPolicyDto : CreateListingPolicyDto
{
}
