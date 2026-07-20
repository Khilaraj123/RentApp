using System;
using System.ComponentModel.DataAnnotations;

namespace RentApp.Application.DTOs.Agreements;

public class CreateRentalAgreementDto
{
    [Required]
    public Guid BookingId { get; set; }
    
    [Required]
    public Guid ListingId { get; set; }
    
    [Required]
    public Guid OwnerId { get; set; }
    
    [Required]
    public Guid RenterId { get; set; }
    
    [Required]
    public string ListingTitle { get; set; } = string.Empty;
    
    [Required]
    public decimal RentalAmount { get; set; }
    
    [Required]
    public string RentalCurrency { get; set; } = string.Empty;
    
    [Required]
    public decimal DepositAmount { get; set; }
    
    [Required]
    public string DepositCurrency { get; set; } = string.Empty;
    
    [Required]
    public DateTime RentalStartDate { get; set; }
    
    [Required]
    public DateTime RentalEndDate { get; set; }
    
    [Required]
    public string TermsAndConditions { get; set; } = string.Empty;
    
    [Required]
    public string DamagePolicy { get; set; } = string.Empty;
    
    [Required]
    public string CancellationPolicy { get; set; } = string.Empty;
}
