using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using RentApp.Application.Common.Options;
using RentApp.Application.DTOs.Users.Auth;
using RentApp.Application.Interfaces.Users;
using RentApp.Domain.Entities.Users;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RentApp.Infrastructure.User
{
    public class JwtService : IJwtService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtOptions _options;

        public JwtService(UserManager<ApplicationUser> userManager, JwtOptions options)
        {
            _userManager = userManager;
            _options = options;
        }

        public async Task<AccessTokenResult> GenerateAccessTokenAsync(ApplicationUser user, CancellationToken cancellationToken = default)
        {
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

                new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName ?? string.Empty),
            };

            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiresAt = DateTime.UtcNow.AddMinutes(_options.AccessTokenExpirationMinutes);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiresAt,
                signingCredentials: credentials
                );

            return new AccessTokenResult
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = expiresAt
            };
        }
    }
}
