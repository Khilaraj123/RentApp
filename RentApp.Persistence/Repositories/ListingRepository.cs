using RentApp.Domain.Entities.Listings;
using RentApp.Domain.Repositories;
using RentApp.Persistence.DbContext;

namespace RentApp.Persistence.Repositories
{
    public class ListingRepository : BaseRepository<Listing>, IListingRepository
    {
        public ListingRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}