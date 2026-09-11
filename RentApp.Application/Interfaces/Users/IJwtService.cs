using RentApp.Application.DTOs.Users.Auth;
using RentApp.Domain.Entities.Users;

namespace RentApp.Application.Interfaces.Users
{
    public interface IJwtService
    {
        Task<AccessTokenResult> GenerateAccessTokenAsync(
        ApplicationUser user,
        CancellationToken cancellationToken = default);
    }
}
