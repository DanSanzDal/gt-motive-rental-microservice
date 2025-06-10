using FluentAssertions;
using GtMotive.Estimate.Microservice.Api.Controllers;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using GtMotive.Estimate.Microservice.Api.DTOs.Responses;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.InfrastructureTests.Infrastructure.Controllers
{
    /// <summary>
    /// Pruebas para el controlador de vehículos.
    /// </summary>
    public class VehicleControllerTests
    {
        private readonly Mock<IVehicleRepository> _vehicleRepositoryMock;
        private readonly VehiclesController _controller;

        /// <summary>
        /// Initializes a new instance of the <see cref="VehicleControllerTests"/> class.
        /// </summary>
        public VehicleControllerTests()
        {
            _vehicleRepositoryMock = new Mock<IVehicleRepository>();

            // Crear una implementación de prueba
            var vehicleService = new TestVehicleService(_vehicleRepositoryMock.Object);

            // Configurar el ServiceProvider
            var services = new ServiceCollection();
            services.AddSingleton(_vehicleRepositoryMock.Object);
            services.AddSingleton<VehicleService>(vehicleService);

            var serviceProvider = services.BuildServiceProvider();

            // Crear un HttpContext
            var httpContext = new DefaultHttpContext
            {
                RequestServices = serviceProvider
            };

            // Crear el controlador y asignarle el HttpContext
            _controller = new VehiclesController()
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = httpContext
                }
            };
        }

        /// <summary>
        /// Prueba que crear un vehículo.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        [Fact]
        public async Task CreateVehicleWithValidModelReturnsCreated()
        {
            // Arrange
            var request = new CreateVehicleRequest
            {
                VIN = "1HGCM82633A123456", // 17 caracteres
                LicensePlate = "ABC123",
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2023,
                ManufactureDate = DateTime.Now.AddYears(-1) // 1 año de antigüedad
            };

            // Act
            var result = await _controller.CreateVehicle(request);

            // Assert
            var createdResult = result.Should().BeAssignableTo<ObjectResult>().Subject;
            createdResult.StatusCode.Should().Be(201);

            var vehicleResponse = createdResult.Value.Should().BeOfType<VehicleResponse>().Subject;
            vehicleResponse.VIN.Should().Be(request.VIN);
            vehicleResponse.Brand.Should().Be(request.Brand);
            vehicleResponse.Model.Should().Be(request.Model);
            vehicleResponse.LicensePlate.Should().Be(request.LicensePlate);
        }

        /// <summary>
        /// Prueba que crear un vehículo con un modelo inválido devuelve BadRequest.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        [Fact]
        public async Task CreateVehicleWithInvalidModelReturnsBadRequest()
        {
            // Arrange
            var request = new CreateVehicleRequest
            {
                VIN = "12345", // VIN demasiado corto
                LicensePlate = string.Empty,
                Brand = string.Empty,
                Model = string.Empty,
                Year = 0,
                ManufactureDate = DateTime.MinValue
            };

            // Agregar errores al ModelState para simular una validación fallida
            _controller.ModelState.AddModelError("VIN", "El VIN debe tener 17 caracteres");

            // Act
            var result = await _controller.CreateVehicle(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        /// <summary>
        /// Prueba que crear un vehículo con más de 5 años de antigüedad devuelve BadRequest.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        [Fact]
        public async Task CreateVehicleWithVehicleOlderThan5YearsReturnsBadRequest()
        {
            // Arrange
            var request = new CreateVehicleRequest
            {
                VIN = "1HGCM82633A123456",
                LicensePlate = "ABC123",
                Brand = "Toyota",
                Model = "Corolla",
                Year = 2023,
                ManufactureDate = DateTime.Now.AddYears(-6) // 6 años de antigüedad
            };

            // Act
            var result = await _controller.CreateVehicle(request);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult?.Value.Should().NotBeNull();
            badRequestResult?.Value?.ToString().Should().Contain("Los vehículos no pueden tener más de 5 años de antigüedad");
        }

        /// <summary>
        /// Prueba que obtener vehículos disponibles devuelve OK con una lista de vehículos.
        /// </summary>
        /// <returns>Una tarea que representa la operación asíncrona.</returns>
        [Fact]
        public async Task GetAvailableVehiclesReturnsOkWithVehicles()
        {
            // Arrange - Crear vehículos para la prueba
            var vehicles = new List<VehicleEntity>
            {
                CreateTestVehicle("1HGCM82633A123456", "ABC123", "Toyota", "Corolla", 2023, DateTime.Now.AddYears(-1)),
                CreateTestVehicle("5YJSA1E11GF176188", "XYZ789", "Tesla", "Model S", 2022, DateTime.Now.AddMonths(-6))
            };

            _vehicleRepositoryMock
                .Setup(r => r.GetAvailableVehiclesAsync())
                .ReturnsAsync(vehicles);

            // Act
            var result = await _controller.GetAvailableVehicles();

            // Assert
            var okResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okResult.StatusCode.Should().Be(200);

            var vehicleResponses = okResult.Value.Should().BeAssignableTo<IEnumerable<VehicleResponse>>().Subject;
            vehicleResponses.Should().HaveCount(2);
        }

        /// <summary>
        /// Crea un vehículo de prueba con los valores especificados.
        /// </summary>
        private static VehicleEntity CreateTestVehicle(string vin, string licensePlateValue, string brand, string model, int year, DateTime manufactureDate)
        {
            var vehicleId = new VehicleId(Guid.NewGuid());
            var vinObject = new VIN(vin);
            var specs = new VehicleSpecs(brand, model, year, manufactureDate, vinObject);
            var licensePlate = new LicensePlate(licensePlateValue);

            return new VehicleEntity(vehicleId, specs, licensePlate);
        }

        /// <summary>
        /// Implementación de VehicleService específica para pruebas.
        /// </summary>
        private sealed class TestVehicleService(IVehicleRepository vehicleRepository) : VehicleService(vehicleRepository)
        {
            // Sobrescribimos el método que utiliza el controlador para crear vehículos
            public override Task<VehicleEntity> CreateVehicleAsync(
                VIN vin,
                LicensePlate licensePlate,
                string make,
                string model,
                int year,
                DateTime manufactureDate)
            {
                if (DateTime.Now.Year - manufactureDate.Year > 5)
                {
                    throw new VehicleTooOldException("Los vehículos no pueden tener más de 5 años de antigüedad");
                }

                var vehicleId = new VehicleId(Guid.NewGuid());
                var specs = new VehicleSpecs(make, model, year, manufactureDate, vin);
                var vehicle = new VehicleEntity(vehicleId, specs, licensePlate);

                return Task.FromResult(vehicle);
            }

            public override Task<VehicleEntity> CreateVehicleAsync(
                string vinValue,
                string licensePlateValue,
                string make,
                string model,
                int year,
                DateTime manufactureDate)
            {
                if (DateTime.Now.Year - manufactureDate.Year > 5)
                {
                    throw new VehicleTooOldException("Los vehículos no pueden tener más de 5 años de antigüedad");
                }

                var vin = new VIN(vinValue);
                var licensePlate = new LicensePlate(licensePlateValue);

                var vehicleId = new VehicleId(Guid.NewGuid());
                var specs = new VehicleSpecs(make, model, year, manufactureDate, vin);
                var vehicle = new VehicleEntity(vehicleId, specs, licensePlate);

                return Task.FromResult(vehicle);
            }
        }
    }
}
