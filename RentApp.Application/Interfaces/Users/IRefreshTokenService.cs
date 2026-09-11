using RentApp.Application.DTOs.Users.Auth;
using RentApp.Domain.Entities.Users;

namespace RentApp.Application.Interfaces.Users
{
    public interface IRefreshTokenService
    {
        Task<RefreshTokenResult> CreateAsync(
            Guid userId,
            string? ipAddress,
            Guid? tokenFamilyId = null,
            CancellationToken cancellationToken = default);

        Task<RefreshToken?> ValidateAsync(
            string refreshToken,
            CancellationToken cancellationToken = default);

        Task<RefreshTokenResult> RotateAsync(
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
