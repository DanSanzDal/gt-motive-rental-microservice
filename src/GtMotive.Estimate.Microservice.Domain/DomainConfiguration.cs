using Microsoft.Extensions.DependencyInjection;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Customers.Services;

namespace GtMotive.Estimate.Microservice.Domain
{
    public static class DomainConfiguration
    {
        public static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            // Registrar Domain Services
            services.AddScoped<IVehicleService, VehicleService>();
            services.AddScoped<IRentalService, RentalService>();
            services.AddScoped<ICustomerService, CustomerService>();

            services.AddScoped(sp => (VehicleService)sp.GetRequiredService<IVehicleService>());
            services.AddScoped(sp => (RentalService)sp.GetRequiredService<IRentalService>());
            services.AddScoped(sp => (CustomerService)sp.GetRequiredService<ICustomerService>());

            return services;
        }
    }
}
