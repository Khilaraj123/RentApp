using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using RentApp.Domain.Constants;
using RentApp.Domain.Entities.Users;

namespace RentApp.Persistence.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAsync(UserManager<ApplicationUser> userManager)
        {
            var email = "admintest@gmail.com";

            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = email,
                    Email = email,
                    FirstName = "System",
                    LastName = "Admin",
                    EmailConfirmed = true
                };

                user.VerifyEmail();

                var result = await userManager.CreateAsync(user, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, Roles.Admin);
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}
