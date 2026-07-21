using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Enums;
using RentApp.Domain.ValueObjects;

namespace RentApp.Domain.DomainServices
{
    public interface IBookingDomainService
    {
        Task<bool> IsListingAvailableAsync(Listing listing, RentalPeriod period, int quantity, CancellationToken cancellationToken = default);
        Task<Booking> CreateBookingAsync(Guid renterId, Listing listing, RentalPeriod period, int quantity, Money totalAmount, Money? depositAmount, DeliveryOption? deliveryOption, CancellationToken cancellationToken = default);
    }
}
