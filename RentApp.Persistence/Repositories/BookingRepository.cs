using RentApp.Domain.Entities.Bookings;
using RentApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RentApp.Persistence.Repositories
{
    public class BookingRepository : IBaseRepository<Booking>
    {
        public Task<Booking?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) => Task.FromResult<Booking?>(null);
        public Task<IReadOnlyList<Booking>> GetAllAsync(CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<Booking>>(new List<Booking>());
        public Task<Booking> AddAsync(Booking entity, CancellationToken cancellationToken = default) => Task.FromResult(entity);
        public Task<IReadOnlyList<Booking>> AddRangeAsync(IEnumerable<Booking> entities, CancellationToken cancellationToken = default) => Task.FromResult<IReadOnlyList<Booking>>(new List<Booking>());
        public void Update(Booking entity) {}
        public void Delete(Booking entity) {}
    }
}