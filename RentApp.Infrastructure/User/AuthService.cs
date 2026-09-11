using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RentApp.Application.Common.Exceptions;
using RentApp.Application.DTOs.Users;
using RentApp.Application.DTOs.Users.Auth;
using RentApp.Application.Interfaces.Users;
using RentApp.Domain.DomainEvents.User;
using RentApp.Domain.Entities.Users;
using RentApp.Domain.Repositories;

namespace RentApp.Infrastructure.User;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AuthService> _logger;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AuthService> logger, IUnitOfWork unitOfWork, IJwtService jwtService, IRefreshTokenService refreshTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto dto, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(dto.Website))
        {
            _logger.LogWarning("Bot detected");
            throw new InvalidOperationException("Invalid login attempt.");
        }

        if (string.IsNullOrWhiteSpace(dto.EmailOrPhone))
        {
            throw new UnAuthorizedException("Email or phone is required.");
        }

        var identifier = dto.EmailOrPhone.Trim();

        var user = await _userManager.FindByEmailAsync(identifier);

        if(user == null)
        {
            user = await _userManager.Users
                    .FirstOrDefaultAsync(
                        x => x.PhoneNumber == identifier,
                        cancellationToken);
        }

        if (user == null || user.IsDeleted)
        {
            throw new UnAuthorizedException("Invalid email or password.");
        }

        var result = await _signInManager.PasswordSignInAsync(user, dto.Password, true, true);

        if (!result.Succeeded)
        {
            throw new UnAuthorizedException("Invalid email or password.");
        }

        var accessToken = await _jwtService.GenerateAccessTokenAsync(user, cancellationToken);
        var refreshToken = await _refreshTokenService.CreateAsync(user.Id, ipAddress, cancellationToken: cancellationToken);

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles
            .Contains("Admin") ? "Admin" : roles.OrderBy(x => x)
                .FirstOrDefault() ?? "";
        
        return new LoginResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? "",
            FullName = user.FullName,
            Role = role,
            AccessToken = accessToken.Token,
            AccessTokenExpiresAt = accessToken.ExpiresAt,
            RefreshToken = refreshToken.Token,
            RefreshTokenExpiresAt = refreshToken.ExpiresAt
        };
    }

    public async Task<LoginResponseDto> RefreshTokenAsync(string refreshToken, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        var existingToken = await _refreshTokenService.ValidateAsync(refreshToken, cancellationToken);

        if (existingToken == null)
        {
            throw new UnAuthorizedException("Invalid refresh token.");
        }

        if (!existingToken.IsActive)
        {
            _logger.LogWarning("Refresh token reuse detected for user {UserId}", existingToken.UserId);

            await _refreshTokenService.RevokeAllAsync(
                existingToken.UserId,
                ipAddress,
                "Refresh token reuse detected",
                cancellationToken);

            throw new UnAuthorizedException("Refresh token is no longer valid.");
        }

        var user = await _userManager
            .FindByIdAsync(existingToken.UserId.ToString());

        if (user == null || user.IsDeleted || !user.IsEnabled)
        {
            throw new UnAuthorizedException("User is no longer active.");
        }

        var newRefreshToken = await _refreshTokenService.RotateAsync(existingToken, ipAddress, cancellationToken);
        var newAccessToken = await _jwtService.GenerateAccessTokenAsync(user, cancellationToken);
        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.Contains("Admin") ? "Admin" : roles.OrderBy(x => x).FirstOrDefault() ?? "";

        return new LoginResponseDto
        {
            Id = user.Id,
            Email = user.Email ?? "",
            FullName = user.FullName,
            Role = role,
            AccessToken = newAccessToken.Token,
            AccessTokenExpiresAt = newAccessToken.ExpiresAt,
            RefreshToken = newRefreshToken.Token,
            RefreshTokenExpiresAt = newRefreshToken.ExpiresAt
        };
    }

    public async Task RegisterAsync(RegisterRequestDto dto, CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(dto.Website))
        {
            _logger.LogWarning("Bot detected");
            throw new InvalidOperationException("Registration failed.");
        }

        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        var roleResult = await _userManager.AddToRoleAsync(user, "User");
        if (!roleResult.Succeeded)
        {
            throw new InvalidOperationException("Failed to assign default role: " + string.Join(", ", roleResult.Errors.Select(x => x.Description)));
        }

        user.AddDomainEvent(new UserRegisteredEvent(user.Id, user.FullName, user.Email));
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public Task<List<UserDto>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task LogoutAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await _refreshTokenService.RevokeAllAsync(userId, ipAddress, "User logged out", cancellationToken);
        await _signInManager.SignOutAsync();
    }

    public async Task LogoutFromAllDevicesAsync(Guid userId, string? ipAddress = null, CancellationToken cancellationToken = default)
    {
        await _refreshTokenService
             .RevokeAllAsync(userId, ipAddress, "User logged out from all devices", cancellationToken);
        await _signInManager.SignOutAsync();
    }
}
