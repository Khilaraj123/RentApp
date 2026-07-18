using System.Linq;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.DomainServices
{
    public class PricingDomainService : IPricingDomainService
    {
        public Money CalculateTotalCost(Listing listing, RentalPeriod period, int quantity)
        {
            // Simple MVP Pricing Logic:
            // Searches for a daily pricing rule. If found, calculates cost based on days.
            
            var dailyRule = listing.PricingRules.FirstOrDefault(r => r.Type == "PerDay");
            
            if (dailyRule != null)
            {
                int days = period.TotalDays == 0 ? 1 : period.TotalDays;
                decimal totalAmount = dailyRule.Price.Amount * days * quantity;
                return new Money(totalAmount, dailyRule.Price.Currency);
            }

            // Fallback for MVP if no daily rule exists
            return Money.Zero("USD");
        }
    }
}
