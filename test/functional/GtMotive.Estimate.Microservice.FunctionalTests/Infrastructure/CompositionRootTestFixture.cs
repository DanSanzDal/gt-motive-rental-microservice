using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api;
using GtMotive.Estimate.Microservice.Api.Presenters;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Infrastructure;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Mongo2Go;
using MongoDB.Driver;
using Xunit;

[assembly: CLSCompliant(false)]

namespace GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure
{
    public sealed class CompositionRootTestFixture : IDisposable, IAsyncLifetime
    {
        private readonly ServiceProvider _serviceProvider;
        private readonly MongoDbRunner _mongoRunner;
        private readonly MongoClient _mongoClient;
        private bool _disposed;

        // Constructor
        public CompositionRootTestFixture()
        {
            // Iniciar MongoDB en memoria usando Mongo2Go
            _mongoRunner = MongoDbRunner.Start();

            // Crear un MongoClient una sola vez para toda la vida del fixture
            _mongoClient = new MongoClient(_mongoRunner.ConnectionString);

            // Crear configuración con la cadena de conexión de MongoDB en memoria
            var inMemorySettings = new Dictionary<string, string>
            {
                { "MongoDB:ConnectionString", _mongoRunner.ConnectionString },
                { "MongoDB:Database", "TestDb" }
            };

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddInMemoryCollection(inMemorySettings)
                .AddEnvironmentVariables()
                .Build();

            var services = new ServiceCollection();
            Configuration = configuration;
            ConfigureServices(services, configuration);

            // Registrar servicios de dominio
            RegisterDomainServicesIfNeeded(services);

            // Registrar manualmente el manejador para CreateRentalCommand
            services.AddTransient<IRequestHandler<CreateRentalCommand, IPresenter>, CreateRentalCommandHandler>();

            services.AddSingleton<IConfiguration>(configuration);
            _serviceProvider = services.BuildServiceProvider();
        }

        public IConfiguration Configuration { get; }

        public string MongoConnectionString => _mongoRunner.ConnectionString;

        public async Task InitializeAsync()
        {
            // Limpiar todas las colecciones antes de cada prueba
            var database = _mongoClient.GetDatabase("TestDb");

            // Obtener todas las colecciones y limpiarlas
            var collections = await database.ListCollectionNamesAsync();
            var collectionList = await collections.ToListAsync();

            foreach (var collection in collectionList)
            {
                await database.DropCollectionAsync(collection);
            }

            await Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            await Task.CompletedTask;
        }

        public async Task UsingHandlerForRequest<TRequest>(Func<IRequestHandler<TRequest, Unit>, Task> handlerAction)
            where TRequest : IRequest
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, Unit>>();

            await handlerAction.Invoke(handler);
        }

        public async Task UsingHandlerForRequestResponse<TRequest, TResponse>(Func<IRequestHandler<TRequest, TResponse>, Task> handlerAction)
            where TRequest : IRequest<TResponse>
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<IRequestHandler<TRequest, TResponse>>();

            Debug.Assert(handler != null, "The requested handler has not been registered");

            await handlerAction.Invoke(handler);
        }

        public async Task UsingRepository<TRepository>(Func<TRepository, Task> handlerAction)
        {
            ArgumentNullException.ThrowIfNull(handlerAction);

            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetRequiredService<TRepository>();

            Debug.Assert(handler != null, "The requested handler has not been registered");

            await handlerAction.Invoke(handler);
        }

        public async Task UsingService<TService>(Func<TService, Task> serviceAction)
        {
            ArgumentNullException.ThrowIfNull(serviceAction);

            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<TService>();

            await serviceAction.Invoke(service);
        }

        // Método de conveniencia para obtener la base de datos
        public IMongoDatabase GetDatabase()
        {
            return _mongoClient.GetDatabase("TestDb");
        }

        // Método de conveniencia para obtener una colección
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return GetDatabase().GetCollection<T>(collectionName);
        }

        // Método para verificar si un servicio está registrado
        public bool IsServiceRegistered<T>()
        {
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetService<T>();
            return service != null;
        }

        // Método para verificar si un manejador está registrado
        public bool IsHandlerRegistered<TRequest, TResponse>()
            where TRequest : IRequest<TResponse>
        {
            using var scope = _serviceProvider.CreateScope();
            var handler = scope.ServiceProvider.GetService<IRequestHandler<TRequest, TResponse>>();
            return handler != null;
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _serviceProvider.Dispose();
                _mongoRunner.Dispose();
                _disposed = true;
            }
        }

        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddApiServices();
            services.AddLogging();
            services.AddInfrastructure(configuration);

            // Registrar MediatR
            services.AddMediatR(typeof(CreateRentalCommand).Assembly);
        }

        private static void RegisterDomainServicesIfNeeded(IServiceCollection services)
        {
            // Registrar RentalService si no está ya registrado
            if (services.All(s => s.ServiceType != typeof(IRentalService)))
            {
                services.AddTransient<IRentalService, RentalService>();
            }

            // Registrar VehicleService si no está ya registrado
            if (services.All(s => s.ServiceType != typeof(IVehicleService)))
            {
                services.AddTransient<IVehicleService, VehicleService>();
            }

            // Registrar el presentador
            if (services.All(s => s.ServiceType != typeof(ICreateRentalOutputPort)))
            {
                services.AddTransient<ICreateRentalOutputPort, CreateRentalPresenter>();
            }

            services.AddScoped<IPresenter>(sp => sp.GetRequiredService<ICreateRentalOutputPort>());
        }
    }
}
