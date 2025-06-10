using Microsoft.Extensions.DependencyInjection;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers;
using MediatR;
using GtMotive.Estimate.Microservice.Domain.Customers;

namespace GtMotive.Estimate.Microservice.ApplicationCore
{
    public static class ApplicationCoreConfiguration
    {
        public static IServiceCollection AddUseCases(this IServiceCollection services)
        {
            // Registra todos los handlers de MediatR
            services.AddMediatR(typeof(ApplicationCoreConfiguration).Assembly);

            // Registra las factories necesarias
            services.AddScoped<ICustomerFactory, CustomerFactory>();

            // Registra los casos de uso
            services.AddScoped<CreateVehicleUseCase>();
            services.AddScoped<RentVehicleUseCase>();
            services.AddScoped<ReturnVehicleUseCase>();
            services.AddScoped<GetAvailableVehiclesUseCase>();
            services.AddScoped<CreateRentalUseCase>();
            services.AddScoped<CompleteRentalUseCase>();
            services.AddScoped<GetActiveRentalsUseCase>();
            services.AddScoped<GetRentalsByCustomerUseCase>();
            services.AddScoped<CreateCustomerUseCase>();

            return services;
        }
    }
}
