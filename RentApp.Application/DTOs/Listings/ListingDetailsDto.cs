using System;
using System.Collections.Generic;

namespace RentApp.Application.DTOs.Listings;

public class ListingDetailsDto : ListingDto
{
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
    
    public string? OwnerEmail { get; set; }
    public string? OwnerPhoneNumber { get; set; }
    
    public ListingPolicyDto? Policy { get; set; }
    public List<ListingImageDto> Images { get; set; } = new();
    public List<PricingRuleDto> PricingRules { get; set; } = new();
    public List<AvailabilityRuleDto> AvailabilityRules { get; set; } = new();
}
