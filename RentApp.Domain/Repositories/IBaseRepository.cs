using RentApp.Domain.Common;
using RentApp.Domain.Common.Pagination;

namespace RentApp.Domain.Repositories
{
    public interface IBaseRepository<T> where T : AggregateRoot
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<PagedResult<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken = default);

        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        void Update(T entity);
        void UpdateRange(IEnumerable<T> entities);

        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        IQueryable<T> QueryableTracking();
        IQueryable<T> QueryableNoTracking();
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

        Task<bool> HardDeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
