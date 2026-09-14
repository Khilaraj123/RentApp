using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Listing?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(l => l.Category)
                .Include(l => l.Owner)
                .Include(l => l.Images)
                .Include(l => l.PricingRules)
                .Include(l => l.AvailabilityRules)
                .Include(l => l.Policy)
                .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }

        public async Task<Listing?> GetBySlugWithDetailsAsync(string slug, CancellationToken cancellationToken = default)
        {
            var normalizedSlug = slug.Trim().ToLowerInvariant();
            return await _dbSet
                .Include(l => l.Category)
                .Include(l => l.Owner)
                .Include(l => l.Images)
                .Include(l => l.PricingRules)
                .Include(l => l.AvailabilityRules)
                .Include(l => l.Policy)
                .FirstOrDefaultAsync(l => l.Slug == normalizedSlug, cancellationToken);
        }

        public async Task<bool> SlugExistsAsync(string slug, Guid? excludeListingId = null, CancellationToken cancellationToken = default)
        {
            var normalizedSlug = slug.Trim().ToLowerInvariant();
            var query = _dbSet.AsNoTracking().Where(l => l.Slug == normalizedSlug);
            if (excludeListingId.HasValue)
            {
                query = query.Where(l => l.Id != excludeListingId.Value);
            }
            return await query.AnyAsync(cancellationToken);
        }
    }
}