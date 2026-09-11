using Microsoft.AspNetCore.Identity;
using RentApp.Domain.Constants;

namespace RentApp.Persistence.Seed
{
    public static class RoleSeeder
    {
        public static async Task SeedAsync(RoleManager<IdentityRole<Guid>> roleManager)
        {
            string[] roles = new[]
            {
                Roles.Admin,
                Roles.Owner,
                Roles.Renter,
                Roles.User
            };

            foreach (var roleName in roles)
            {
                var exists = await roleManager.RoleExistsAsync(roleName);

                if (!exists)
                {
                    var role = new IdentityRole<Guid>
                    {
                        Id = Guid.NewGuid(),
                        Name = roleName,
                        NormalizedName = roleName.ToUpperInvariant()
                    };
                    await roleManager.CreateAsync(role);
                }
            }
        }
    }
}
