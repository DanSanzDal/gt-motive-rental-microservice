using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Repositories;
using GtMotive.Estimate.Microservice.Infrastructure.Configuration;
using GtMotive.Estimate.Microservice.Infrastructure.Logging;
using GtMotive.Estimate.Microservice.Infrastructure.Telemetry;
using Microsoft.Extensions.Options;


namespace GtMotive.Estimate.Microservice.Infrastructure
{
    public static class InfrastructureConfiguration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MongoDbSettings>(configuration.GetSection(MongoDbSettings.SectionName));

            var appSettingsSection = configuration.GetSection("AppSettings");
            if (appSettingsSection.Exists())
            {
                services.Configure<AppSettings>(appSettingsSection);
            }

            // Configurar MongoDB
            services.AddMongoDB();

            // Registrar Repositories
            services.AddRepositories();

            // Registrar Logging y Telemetría
            services.AddInfrastructureLogging();
            services.AddTelemetry();

            services.AddSingleton<AppTelemetry>();

            return services;
        }

        private static IServiceCollection AddMongoDB(this IServiceCollection services)
        {
            services.AddSingleton<IMongoClient>(serviceProvider =>
            {
                var settings = serviceProvider
                    .GetRequiredService<IOptions<MongoDbSettings>>()
                    .Value;

                return new MongoClient(settings.ConnectionString);
            });

            services.AddScoped(serviceProvider =>
            {
                var settings = serviceProvider
                    .GetRequiredService<IOptions<MongoDbSettings>>()
                    .Value;

                var client = serviceProvider.GetRequiredService<IMongoClient>();
                return client.GetDatabase(settings.DatabaseName);
            });

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IVehicleRepository, VehicleRepository>();
            services.AddScoped<IRentalRepository, RentalRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();

            return services;
        }

        public class InfrastructureLoggingCategory { }

        private static IServiceCollection AddInfrastructureLogging(this IServiceCollection services)
        {
            services.AddScoped<LoggerAdapter<InfrastructureLoggingCategory>>();
            return services;
        }

        private static IServiceCollection AddTelemetry(this IServiceCollection services)
        {
            services.AddScoped<AppTelemetry>();
            return services;
        }
    }
}
