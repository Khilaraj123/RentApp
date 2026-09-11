using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using RentApp.Application.Common.Options;
using RentApp.Application.DTOs.Users.Auth;
using RentApp.Application.Interfaces.Users;
using RentApp.Domain.Entities.Users;
using RentApp.Domain.Repositories;

namespace RentApp.Infrastructure.User
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBaseRepository<RefreshToken> _repository;
        private readonly JwtOptions _options;

        public RefreshTokenService(IUnitOfWork unitOfWork, IBaseRepository<RefreshToken> repository, JwtOptions options)
        {
            _unitOfWork = unitOfWork;
            _repository = repository;
            _options = options;
        }

        public async Task<RefreshTokenResult> CreateAsync(Guid userId, string? ipAddress, Guid? tokenFamilyId = null, CancellationToken cancellationToken = default)
        {
            var rawToken = GenerateSecureToken();

            var refreshToken = new RefreshToken(
                userId,
                rawToken,
                HashToken(rawToken),
                tokenFamilyId ?? Guid.NewGuid(),
                DateTime.UtcNow.AddDays(_options.RefreshTokenExpirationDays),
                ipAddress);

            await _repository.AddAsync(refreshToken, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new RefreshTokenResult
            {
                Id = refreshToken.Id,
                Token = rawToken,
                TokenFamilyId = refreshToken.TokenFamilyId,
                ExpiresAt = refreshToken.ExpiresAt
            };
        }

        public Task RevokeAllAsync(Guid userId, string? ipAddress, string reason, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task RevokeAsync(RefreshToken token, string? ipAddress, string reason, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenResult> RotateAsync(RefreshToken currentToken, string? ipAddress, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshToken?> ValidateAsync(string refreshToken, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        private static string GenerateSecureToken()
        {
            var bytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(bytes);
            return Convert.ToBase64String(bytes);
        }

        private static string HashToken(string token)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(token));
            return Convert.ToBase64String(bytes);
        }
    }
}
