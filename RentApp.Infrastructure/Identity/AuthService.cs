using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RentApp.Application.DTOs.Auth;
using RentApp.Application.DTOs.Users;
using RentApp.Application.Interfaces.Identity;
using RentApp.Domain.Common;
using RentApp.Domain.Entities.Users;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RentApp.Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthService> _logger;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    public Task<List<UserDto>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task LogoutAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task LogoutFromAllDevicesAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
