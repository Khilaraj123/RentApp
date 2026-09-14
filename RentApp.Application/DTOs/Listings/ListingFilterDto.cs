using System;
using RentApp.Domain.Enums;
using RentApp.Domain.Enums.Listing;

namespace RentApp.Application.DTOs.Listings;

public class ListingFilterDto
{
    public Guid? CategoryId { get; set; }
    public Guid? OwnerId { get; set; }
    public ListingStatus? Status { get; set; }
    public ListingCondition? Condition { get; set; }
    public DeliveryOption? DeliveryOption { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public RentalUnit? RentalUnit { get; set; }
    public string? SearchTerm { get; set; }
    public string? SortBy { get; set; } // "price_asc", "price_desc", "rating", "newest", "views", "popular"
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 12;
}
