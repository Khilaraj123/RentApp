using Microsoft.AspNetCore.Identity;
using RentApp.Application.DTOs.Auth;
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

    public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public async Task<Result<LoginResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            return Result<LoginResponseDto>.Success(new LoginResponseDto
            {
                // Assign any relevant properties here
            });
        }

        return Result<LoginResponseDto>.Failure("Invalid login attempt.");
    }

    public async Task<Result> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser(request.FirstName, request.LastName, request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);
        
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Result.Success();
        }

        var error = result.Errors.FirstOrDefault()?.Description ?? "Failed to register user.";
        return Result.Failure(error);
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }
}
