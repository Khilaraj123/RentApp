using System;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.DTOs.Listings;

public class ListingCardDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? PrimaryImageUrl { get; set; }
    public decimal? BasePrice { get; set; }
    public RentalUnit? BasePriceUnit { get; set; }
    public string Currency { get; set; } = "USD";
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public ListingCondition Condition { get; set; }
    public ListingStatus Status { get; set; }
    public decimal AverageRating { get; set; }
    public int ReviewCount { get; set; }
    public bool RequiresOwnerApproval { get; set; }
    public DeliveryOption DeliveryOption { get; set; }
    public Guid OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
}
