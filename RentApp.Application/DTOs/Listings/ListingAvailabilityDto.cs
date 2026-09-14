using System;

namespace RentApp.Application.DTOs.Listings;

public class ListingAvailabilityDto
{
    public Guid ListingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAvailable { get; set; }
    public int AvailableQuantity { get; set; }
    public string? Reason { get; set; }
}

public class AvailabilityRuleDto
{
    public Guid Id { get; set; }
    public Guid ListingId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAvailable { get; set; }
}

public class CreateAvailabilityRuleDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsAvailable { get; set; }
}
