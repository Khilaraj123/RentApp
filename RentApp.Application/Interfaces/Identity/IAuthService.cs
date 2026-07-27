using RentApp.Application.DTOs.Auth;
using RentApp.Domain.Common;

namespace RentApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task LogoutAsync();
}
