using RentApp.Domain.Entities.Listings;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.DomainServices
{
    public interface IPricingDomainService
    {
        Money CalculateTotalCost(Listing listing, RentalPeriod period, int quantity);
    }
}
