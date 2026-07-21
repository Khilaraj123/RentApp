using Microsoft.AspNetCore.Identity;
using RentApp.Application.DTOs.Auth;
using RentApp.Application.Interfaces.Identity;
using RentApp.Domain.Entities.Users;
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

    public async Task<LoginResponseDto> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default)
    {
        var result = await _signInManager.PasswordSignInAsync(request.Email, request.Password, false, lockoutOnFailure: false);

        if (result.Succeeded)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            return new LoginResponseDto
            {
                // Assign any relevant properties here
            };
        }

        return null;
    }

    public async Task<bool> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default)
    {
        var user = new ApplicationUser(request.FirstName, request.LastName, request.Email);
        var result = await _userManager.CreateAsync(user, request.Password);
        
        return result.Succeeded;
    }
}
