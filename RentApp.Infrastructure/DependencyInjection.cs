using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RentApp.Application.Common.Options;
using RentApp.Application.Interfaces.External;
using RentApp.Application.Interfaces.Users;
using RentApp.Domain.DomainServices.Implementations;
using RentApp.Domain.DomainServices.Interfaces;
using RentApp.Infrastructure.Services.External;
using RentApp.Infrastructure.User;

namespace RentApp.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // JWT Options configuration
            services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<JwtOptions>>().Value);

            // User & Auth Services
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IAuthService, AuthService>();

            // Domain Event Dispatcher
            services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

            // Domain Services
            services.AddScoped<IBookingDomainService, BookingDomainService>();
            services.AddScoped<IPricingDomainService, PricingDomainService>();

            return services;
        }
    }
}
