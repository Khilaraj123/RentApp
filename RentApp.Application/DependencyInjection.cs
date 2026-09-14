using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RentApp.Application.Interfaces.External;

namespace RentApp.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            var eventHandlerType = typeof(IDomainEventHandler<>);
            var assemblyTypes = typeof(DependencyInjection).Assembly.GetTypes()
                .Where(t => !t.IsAbstract && !t.IsInterface);

            foreach (var type in assemblyTypes)
            {
                var interfaces = type.GetInterfaces()
                    .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == eventHandlerType);

                foreach (var @interface in interfaces)
                {
                    services.AddScoped(@interface, type);
                }
            }

            // Services & Helpers
            services.AddScoped<RentApp.Application.Interfaces.Helpers.ISlugGenerator, RentApp.Application.Common.Helpers.SlugGenerator>();
            services.AddScoped<RentApp.Application.Interfaces.Listings.IListingService, RentApp.Application.Services.Listings.ListingService>();

            return services;
        }
    }
}
