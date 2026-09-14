using System;

namespace RentApp.Application.DTOs.Listings;

public class ListingSearchDto : ListingFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? QuantityNeeded { get; set; }
}
