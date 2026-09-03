using RentApp.Application.DTOs.Auth;
using RentApp.Domain.Entities.Users;

namespace RentApp.Application.Interfaces.Users
{
    internal interface IRefreshTokenService
    {
        Task<UserTokenDto> CreateAsync(
        Guid userId,
        string? ipAddress,
        Guid? tokenFamilyId = null,
        CancellationToken cancellationToken = default);

        Task<UserTokenDto?> ValidateAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task<UserTokenDto> RotateAsync(
            RefreshToken currentToken,
            string? ipAddress,
            CancellationToken cancellationToken = default);

        Task RevokeAsync(
        RefreshToken token,
        string? ipAddress,
        string reason,
        CancellationToken cancellationToken = default);

        Task RevokeAllAsync(
            Guid userId,
            string? ipAddress,
            string reason,
            CancellationToken cancellationToken = default);
    }
}
