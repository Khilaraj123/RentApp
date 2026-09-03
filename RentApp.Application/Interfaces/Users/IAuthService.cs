using RentApp.Application.DTOs.Auth;
using RentApp.Application.DTOs.Users;
using RentApp.Domain.Common;

namespace RentApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(
            LoginRequestDto dto,
            string? ipAddress = null,
            CancellationToken cancellationToken = default);

    Task<LoginResponseDto> RefreshTokenAsync(
        string refreshToken,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task RegisterAsync(
        RegisterRequestDto dto,
        CancellationToken cancellationToken = default);

    Task<List<UserDto>> GetUsersInRoleAsync(
        string roleName,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        Guid userId,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);

    Task LogoutFromAllDevicesAsync(
        Guid userId,
        string? ipAddress = null,
        CancellationToken cancellationToken = default);
}
