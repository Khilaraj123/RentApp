using RentApp.Application.DTOs.Auth;
using RentApp.Application.DTOs.Users;

namespace RentApp.Application.Interfaces.Users
{
    public interface IJwtService
    {
        Task<UserTokenDto> GenerateAccessTokenAsync(
        UserDto user,
        CancellationToken cancellationToken = default);
    }
}
