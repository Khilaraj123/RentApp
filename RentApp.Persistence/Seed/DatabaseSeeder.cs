using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RentApp.Domain.Entities.Users;

namespace RentApp.Persistence.Seed
{
    public static class DatabaseSeeder
    {
        public static async Task SeedDatabaseAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var services = scope.ServiceProvider;

            try
            {
                var roleManager = services.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

                await RoleSeeder.SeedAsync(roleManager);
                await AdminSeeder.SeedAsync(userManager);
            }
            catch (Exception ex)
            {
                var logger = services.GetRequiredService<ILoggerFactory>().CreateLogger("DatabaseSeeder");
                logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
    }
}
