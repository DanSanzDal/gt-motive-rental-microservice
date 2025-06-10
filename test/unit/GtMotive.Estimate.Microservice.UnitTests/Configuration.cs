using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Infrastructure.Configuration;
using GtMotive.Estimate.Microservice.UnitTests.Mocks;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace GtMotive.Estimate.Microservice.UnitTests
{
    public static class Configuration
    {
        public static IServiceCollection ConfigureTestServices(IServiceCollection services)
        {
            // Configuración para MongoDB (aunque no se usará con los mocks)
            services.Configure<MongoDbSettings>(options =>
            {
                options.ConnectionString = "mongodb://localhost:27017";
                options.DatabaseName = "RentalUnitTestDb";
            });

            // Registra los repositorios mock
            services.AddScoped<IRentalRepository, MockRentalRepository>();
            services.AddScoped<IVehicleRepository, MockVehicleRepository>();
            services.AddScoped<ICustomerRepository, MockCustomerRepository>();

            return services;
        }

        // Método de ayuda para crear un ServiceProvider para tests
        public static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            ConfigureTestServices(services);

            return services.BuildServiceProvider();
        }
    }
}
