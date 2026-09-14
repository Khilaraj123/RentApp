using System;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.DTOs.Listings;

public class UpdateListingDto
{
    public Guid CategoryId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public ListingCondition Condition { get; set; }
    public int? Year { get; set; }
    public int Quantity { get; set; }
    public DeliveryOption DeliveryOption { get; set; }
    public bool RequiresOwnerApproval { get; set; }

    // Address
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    // SEO
    public string? MetaTitle { get; set; }
    public string? MetaDescription { get; set; }
}
