using RentApp.Domain.Entities.Reviews;
using RentApp.Domain.Repositories;
using RentApp.Persistence.DbContext;

namespace RentApp.Persistence.Repositories
{
    public class ReviewRepository : BaseRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
