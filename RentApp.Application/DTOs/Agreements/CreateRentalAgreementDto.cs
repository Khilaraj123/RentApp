using System;
using System.ComponentModel.DataAnnotations;

namespace RentApp.Application.DTOs.Agreements;

public record CreateRentalAgreementDto
{
    public Guid BookingId { get; set; }
    public Guid ListingId { get; set; }
    public Guid OwnerId { get; set; }
    public Guid RenterId { get; set; }
    public string ListingTitle { get; set; } = string.Empty;
    public decimal RentalAmount { get; set; }
    public string RentalCurrency { get; set; } = string.Empty;
    public decimal DepositAmount { get; set; }
    public string DepositCurrency { get; set; } = string.Empty;
    public DateTime RentalStartDate { get; set; }
    public DateTime RentalEndDate { get; set; }
    public string TermsAndConditions { get; set; } = string.Empty;
    public string DamagePolicy { get; set; } = string.Empty;
    public string CancellationPolicy { get; set; } = string.Empty;
}
