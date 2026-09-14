using RentApp.Domain.Entities.Listings;

namespace RentApp.Domain.Repositories
{
    public interface IListingRepository : IBaseRepository<Listing>
    {
        Task<Listing?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
        Task<Listing?> GetBySlugWithDetailsAsync(string slug, CancellationToken cancellationToken = default);
        Task<bool> SlugExistsAsync(string slug, Guid? excludeListingId = null, CancellationToken cancellationToken = default);
    }
}
