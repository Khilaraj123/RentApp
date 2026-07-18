using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Repositories;
using RentApp.Domain.ValueObjects;
using RentApp.Domain.DomainEvents;

namespace RentApp.Domain.DomainServices
{
    public class BookingDomainService : IBookingDomainService
    {
        private readonly IBookingRepository _bookingRepository;

        public BookingDomainService(IBookingRepository bookingRepository)
        {
            _bookingRepository = bookingRepository;
        }

        public async Task<bool> IsListingAvailableAsync(Listing listing, RentalPeriod period, int quantity, CancellationToken cancellationToken = default)
        {
            // For MVP: Pull all bookings and check overlap. 
            // In a production app, a specialized repository method should perform this query on the database side.
            var allBookings = await _bookingRepository.GetAllAsync(cancellationToken);
            
            var overlappingBookings = allBookings
                .Where(b => b.ListingId == listing.Id)
                .Where(b => b.Status != Enums.BookingStatus.Cancelled && b.Status != Enums.BookingStatus.Rejected)
                .Where(b => b.Period.OverlapsWith(period))
                .ToList();

            var bookedQuantity = overlappingBookings.Sum(b => b.Quantity);

            return (listing.Quantity - bookedQuantity) >= quantity;
        }

        public async Task<Booking> CreateBookingAsync(Guid renterId, Listing listing, RentalPeriod period, int quantity, Money totalAmount, Money? depositAmount, string? deliveryOption, CancellationToken cancellationToken = default)
        {
            var isAvailable = await IsListingAvailableAsync(listing, period, quantity, cancellationToken);
            
            if (!isAvailable)
            {
                throw new InvalidOperationException("The listing is not available for the requested period and quantity.");
            }

            var booking = new Booking(renterId, listing.Id, period, totalAmount, depositAmount, deliveryOption, quantity);
            
            // Raise domain event
            booking.AddDomainEvent(new BookingRequestedEvent(booking.Id, listing.Id, renterId));

            return booking;
        }
    }
}
