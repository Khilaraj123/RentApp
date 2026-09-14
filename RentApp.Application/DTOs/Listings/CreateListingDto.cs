using System;
using System.Collections.Generic;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.DTOs.Listings;

public class CreateListingDto
{
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public ListingCondition Condition { get; set; } = ListingCondition.Good;
    public int? Year { get; set; }
    public int Quantity { get; set; } = 1;
    public DeliveryOption DeliveryOption { get; set; } = DeliveryOption.Pickup;
    public bool RequiresOwnerApproval { get; set; } = true;
    
    // Address
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // Initial Pricing Rules (optional during creation)
    public List<CreatePricingRuleDto>? PricingRules { get; set; }
    
    // Policy
    public CreateListingPolicyDto? Policy { get; set; }

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}
