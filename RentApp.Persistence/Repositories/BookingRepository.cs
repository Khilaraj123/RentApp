using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Repositories;
using RentApp.Persistence.DbContext;

namespace RentApp.Persistence.Repositories
{
    public class BookingRepository : BaseRepository<Booking>, IBookingRepository
    {
        public BookingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}