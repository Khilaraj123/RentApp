using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RentApp.Domain.Entities.Users;
using RentApp.Domain.Repositories;
using RentApp.Persistence.DbContext;

namespace RentApp.Persistence.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
        }

        public async Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Users.AsNoTracking().ToListAsync(cancellationToken);
        }

        public async Task<ApplicationUser> AddAsync(ApplicationUser entity, CancellationToken cancellationToken = default)
        {
            await _context.Users.AddAsync(entity, cancellationToken);
            return entity;
        }

        public async Task<IReadOnlyList<ApplicationUser>> AddRangeAsync(IEnumerable<ApplicationUser> entities, CancellationToken cancellationToken = default)
        {
            var list = entities.ToList();
            await _context.Users.AddRangeAsync(list, cancellationToken);
            return list;
        }

        public void Update(ApplicationUser entity)
        {
            _context.Users.Update(entity);
        }

        public void Delete(ApplicationUser entity)
        {
            _context.Users.Remove(entity);
        }

        public async Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.NormalizedEmail == email.ToUpperInvariant(), cancellationToken);
        }

        public async Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default)
        {
            return !await _context.Users.AnyAsync(u => u.NormalizedEmail == email.ToUpperInvariant(), cancellationToken);
        }
    }
}
