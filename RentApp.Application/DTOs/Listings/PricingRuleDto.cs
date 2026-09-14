using System;
using RentApp.Domain.Enums;

namespace RentApp.Application.DTOs.Listings;

public class PricingRuleDto
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public RentalUnit Unit { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
}

public class CreatePricingRuleDto
{
    public RentalUnit Unit { get; set; }
    public decimal Price { get; set; }
    public string Currency { get; set; } = "USD";
    public int? MinDuration { get; set; }
    public int? MaxDuration { get; set; }
}
