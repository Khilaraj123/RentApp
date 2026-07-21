using RentApp.Application.DTOs.Auth;

namespace RentApp.Application.Interfaces.Identity;

public interface IAuthService
{
    Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<bool> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
}
