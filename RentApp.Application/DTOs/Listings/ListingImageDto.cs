using System;

namespace RentApp.Application.DTOs.Listings;

public class ListingImageDto
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public bool IsPrimary { get; set; }
    public int Order { get; set; }
}
