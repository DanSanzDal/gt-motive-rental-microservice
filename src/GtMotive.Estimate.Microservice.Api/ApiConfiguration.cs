using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;
using GtMotive.Estimate.Microservice.Api.Filters;
using System.Reflection;
using System.IO;
using System;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands;
using GtMotive.Estimate.Microservice.Api.Presenters;

namespace GtMotive.Estimate.Microservice.Api
{
    public static class ApiConfiguration
    {
        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            // Registrar los presenters
            services.AddScoped<ICreateVehicleOutputPort, CreateVehiclePresenter>();
            services.AddScoped<IRentVehicleOutputPort, RentVehiclePresenter>();
            services.AddScoped<ApplicationCore.Vehicles.Commands.IReturnVehicleOutputPort, ReturnVehiclePresenter>();
            services.AddScoped<ApplicationCore.Rentals.Commands.IReturnVehicleOutputPort, ReturnVehiclePresenter>();
            services.AddScoped<IGetAvailableVehiclesOutputPort, GetAvailableVehiclesPresenter>();
            services.AddScoped<ICreateRentalOutputPort, CreateRentalPresenter>();
            services.AddScoped<ICompleteRentalOutputPort, CompleteRentalPresenter>();
            services.AddScoped<IGetActiveRentalsOutputPort, GetActiveRentalsPresenter>();
            services.AddScoped<IGetRentalsByCustomerOutputPort, GetRentalsByCustomerPresenter>();
            services.AddScoped<ICreateCustomerOutputPort, CreateCustomerPresenter>();

            // Configurar controllers
            services.AddControllers(options =>
            {
                options.Filters.Add<ValidationFilter>();
                options.Filters.Add<BusinessExceptionFilter>();
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Deshabilitar validación automática para usar nuestro filtro personalizado
                options.SuppressModelStateInvalidFilter = true;
            });

            // Configurar FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Configurar Swagger/OpenAPI
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Vehicle Rental API",
                    Version = "v1",
                    Description = "API for managing vehicle rentals"
                });

                // Incluir comentarios XML
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }
            });

            return services;
        }
    }
}
