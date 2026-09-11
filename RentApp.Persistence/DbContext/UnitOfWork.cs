using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using RentApp.Application.Interfaces.External;
using RentApp.Domain.Common;
using RentApp.Domain.Repositories;

namespace RentApp.Persistence.DbContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private readonly IDomainEventDispatcher? _domainEventDispatcher;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(ApplicationDbContext context, IDomainEventDispatcher? domainEventDispatcher = null)
        {
            _context = context;
            _domainEventDispatcher = domainEventDispatcher;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (_currentTransaction != null)
            {
                return;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);

                if (_currentTransaction != null)
                {
                    await _currentTransaction.CommitAsync(cancellationToken);
                }
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    await _currentTransaction.DisposeAsync();
                    _currentTransaction = null;
                }
            }
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_domainEventDispatcher != null)
            {
                var entitiesWithEvents = _context.ChangeTracker
                    .Entries<IHasDomainEvents>()
                    .Where(e => e.Entity.DomainEvents != null && e.Entity.DomainEvents.Any())
                    .Select(e => e.Entity)
                    .ToList();

                var domainEvents = entitiesWithEvents
                    .SelectMany(e => e.DomainEvents)
                    .ToList();

                foreach (var entity in entitiesWithEvents)
                {
                    entity.ClearDomainEvents();
                }

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (domainEvents.Any())
                {
                    await _domainEventDispatcher.DispatchAsync(domainEvents, cancellationToken);
                }

                return result;
            }

            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
