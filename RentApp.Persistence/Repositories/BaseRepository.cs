using RentApp.Domain.Common;
using RentApp.Domain.Repositories;

namespace RentApp.Persistence.Repositories
{
    public class BaseRepository : IBaseRepository<AggregateRoot>
    {
        public Task<AggregateRoot> AddAsync(AggregateRoot entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AggregateRoot>> AddRangeAsync(IEnumerable<AggregateRoot> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Delete(AggregateRoot entity)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<AggregateRoot>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<AggregateRoot?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(AggregateRoot entity)
        {
            throw new NotImplementedException();
        }
    }
}
