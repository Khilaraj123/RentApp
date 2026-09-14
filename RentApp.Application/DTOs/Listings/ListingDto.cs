using System;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.DTOs.Listings;

public class ListingDto
{
    public Guid Id { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public ListingCondition Condition { get; set; }
    public int? Year { get; set; }
    public int Quantity { get; set; }
    
    public ListingStatus Status { get; set; }
    public DeliveryOption DeliveryOption { get; set; }
    public bool RequiresOwnerApproval { get; set; }
    
    public string? Street { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    public Guid? CoverImageId { get; set; }
    public string? CoverImageUrl { get; set; }
    
    public int ViewCount { get; set; }
    public int BookingCount { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public DateTime CreatedAt { get; set; }
}
