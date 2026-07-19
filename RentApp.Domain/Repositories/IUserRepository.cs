using System.Threading;
using System.Threading.Tasks;
using RentApp.Domain.Entities.Users;

namespace RentApp.Domain.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ApplicationUser>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<ApplicationUser> AddAsync(ApplicationUser entity, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<ApplicationUser>> AddRangeAsync(IEnumerable<ApplicationUser> entities, CancellationToken cancellationToken = default);
        
        void Update(ApplicationUser entity);
        void Delete(ApplicationUser entity);

        Task<ApplicationUser?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> IsEmailUniqueAsync(string email, CancellationToken cancellationToken = default);
    }
}
