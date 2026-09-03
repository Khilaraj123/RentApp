using RentApp.Application.Common.Pagination.Offset;
using RentApp.Application.DTOs.Users;

namespace RentApp.Application.Interfaces.Users
{
    public interface IUserService
    {
        Task<UserDto?> GetUserByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<List<UserDto>> GetAdminsAsync(CancellationToken cancellationToken = default);

        Task<PagedResult<UserDto>> GetPagedUsersAsync(string? searchTerm, PaginationRequest request, bool isAdmin, CancellationToken cancellationToken = default);
        Task<List<UserDto>> SearchUsersAsync(string searchTerm, bool requireRole, int limit, CancellationToken cancellationToken = default);
        Task<bool> AssignUserRolesAsync(Guid userId, List<string> roles, Guid assignedBy, CancellationToken cancellationToken = default);
        Task<UserDto?> GetByIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<string>> GetUserRolesAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
