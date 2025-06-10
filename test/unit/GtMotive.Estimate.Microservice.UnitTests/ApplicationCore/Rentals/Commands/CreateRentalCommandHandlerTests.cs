using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using Moq;
using Xunit;

namespace GtMotive.Estimate.Microservice.UnitTests.ApplicationCore.Rentals.Commands
{
    public class CreateRentalCommandHandlerTests
    {
        private readonly Mock<IRentalRepository> rentalRepositoryMock;
        private readonly Mock<ICustomerRepository> customerRepositoryMock;
        private readonly Mock<IVehicleRepository> vehicleRepositoryMock;
        private readonly Mock<IRentalService> rentalServiceMock;
        private readonly Mock<IVehicleService> vehicleServiceMock;
        private readonly Mock<ICreateRentalOutputPort> outputPortMock;
        private readonly CreateRentalCommandHandler handler;

        public CreateRentalCommandHandlerTests()
        {
            rentalRepositoryMock = new Mock<IRentalRepository>();
            customerRepositoryMock = new Mock<ICustomerRepository>();
            vehicleRepositoryMock = new Mock<IVehicleRepository>();
            rentalServiceMock = new Mock<IRentalService>();
            vehicleServiceMock = new Mock<IVehicleService>();
            outputPortMock = new Mock<ICreateRentalOutputPort>();

            handler = new CreateRentalCommandHandler(
                rentalRepositoryMock.Object,
                vehicleRepositoryMock.Object,
                customerRepositoryMock.Object,
                rentalServiceMock.Object,
                vehicleServiceMock.Object,
                outputPortMock.Object);
        }

        [Fact]
        public async Task HandleWithValidDataShouldCreateRental()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);

            var command = new CreateRentalCommand(
                customerId,
                vehicleId,
                startDate,
                endDate);

            var customer = CreateTestCustomer(customerId);
            var vehicle = CreateTestVehicle(vehicleId, true);

            var rentalId = new RentalId(Guid.NewGuid());
            var rentalPeriod = new RentalPeriod(startDate, endDate);
            var rental = new Rental(rentalId, new CustomerId(customerId), new VehicleId(vehicleId), rentalPeriod);

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)))
                .ReturnsAsync(customer);

            vehicleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)))
                .ReturnsAsync(vehicle);

            rentalServiceMock
                .Setup(service => service.CreateRentalAsync(
                    It.Is<CustomerId>(id => id.ToGuid() == customerId),
                    It.Is<VehicleId>(id => id.ToGuid() == vehicleId),
                    It.Is<RentalPeriod>(p => p.StartDate == startDate && p.EndDate == endDate)))
                .ReturnsAsync(rental);

            vehicleServiceMock
                .Setup(service => service.RentVehicleAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)))
                .Returns(Task.CompletedTask);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            customerRepositoryMock.Verify(
                repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)),
                Times.Once);

            vehicleRepositoryMock.Verify(
                repo => repo.GetByIdAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)),
                Times.Once);

            rentalRepositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Rental>(r =>
                    r.CustomerId.ToGuid() == customerId &&
                    r.VehicleId.ToGuid() == vehicleId &&
                    r.Period.StartDate == startDate &&
                    r.Period.EndDate == endDate &&
                    r.Status == RentalStatus.Active)),
                Times.Once);

            vehicleServiceMock.Verify(
                service => service.RentVehicleAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)),
                Times.Once);

            outputPortMock.Verify(
                output => output.CreatedHandle(It.IsAny<RentalDto>()),
                Times.Once);

            Assert.NotNull(result);
            Assert.Same(outputPortMock.Object, result);
        }

        [Fact]
        public async Task HandleWithNonExistentCustomerShouldReturnNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);

            var command = new CreateRentalCommand(
                customerId,
                vehicleId,
                startDate,
                endDate);

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)))
                .ReturnsAsync((CustomerEntity)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            outputPortMock.Verify(
                output => output.NotFound(It.Is<string>(s => s.Contains("Customer", StringComparison.Ordinal))),
                Times.Once);

            Assert.NotNull(result);
            Assert.Same(outputPortMock.Object, result);
        }

        [Fact]
        public async Task HandleWithNonExistentVehicleShouldReturnNotFound()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);

            var command = new CreateRentalCommand(
                customerId,
                vehicleId,
                startDate,
                endDate);

            var customer = CreateTestCustomer(customerId);

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)))
                .ReturnsAsync(customer);

            vehicleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)))
                .ReturnsAsync((VehicleEntity)null);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            outputPortMock.Verify(
                output => output.NotFound(It.Is<string>(s => s.Contains("Vehicle", StringComparison.Ordinal))),
                Times.Once);

            Assert.NotNull(result);
            Assert.Same(outputPortMock.Object, result);
        }

        [Fact]
        public async Task HandleWithUnavailableVehicleShouldReturnBadRequest()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);

            var command = new CreateRentalCommand(
                customerId,
                vehicleId,
                startDate,
                endDate);

            var customer = CreateTestCustomer(customerId);
            var vehicle = CreateTestVehicle(vehicleId, false); // Vehículo no disponible

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)))
                .ReturnsAsync(customer);

            vehicleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)))
                .ReturnsAsync(vehicle);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            outputPortMock.Verify(
                output => output.BadRequest(It.Is<string>(s => s.Contains("not available", StringComparison.Ordinal))),
                Times.Once);

            Assert.NotNull(result);
            Assert.Same(outputPortMock.Object, result);
        }

        [Fact]
        public async Task HandleWithInvalidDateRangeShouldReturnBadRequest()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var vehicleId = Guid.NewGuid();
            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(-1); // Fecha fin anterior a fecha inicio

            var command = new CreateRentalCommand(
                customerId,
                vehicleId,
                startDate,
                endDate);

            var customer = CreateTestCustomer(customerId);
            var vehicle = CreateTestVehicle(vehicleId, true); // Vehículo disponible

            customerRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<CustomerId>(id => id.ToGuid() == customerId)))
                .ReturnsAsync(customer);

            vehicleRepositoryMock
                .Setup(repo => repo.GetByIdAsync(It.Is<VehicleId>(id => id.ToGuid() == vehicleId)))
                .ReturnsAsync(vehicle);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            outputPortMock.Verify(
                output => output.BadRequest(It.Is<string>(s => s.Contains("End date must be at least 1 day after start date", StringComparison.Ordinal))),
                Times.Once);

            Assert.NotNull(result);
            Assert.Same(outputPortMock.Object, result);
        }

        private static CustomerEntity CreateTestCustomer(Guid customerId)
        {
            var customerIdObj = new CustomerId(customerId);
            var name = "Test Customer";
            var email = new CustomerEmail("test@customer.com");
            var phoneNumber = new PhoneNumber("+1234567890");
            var drivingLicense = new DrivingLicense("TEST12345", DateTime.UtcNow.AddYears(1));

            return new CustomerEntity(customerIdObj, name, email, phoneNumber, drivingLicense);
        }

        private static VehicleEntity CreateTestVehicle(Guid vehicleId, bool isAvailable)
        {
            // Crear objetos necesarios para una instancia válida de vehículo
            var manufacturingDate = DateTime.UtcNow.AddYears(-2); // Un vehículo de 2 años de edad
            var vin = new VIN("1HGCM82633A123456"); // Un VIN de ejemplo válido
            var specs = new VehicleSpecs("TestMake", "TestModel", DateTime.UtcNow.Year - 2, manufacturingDate, vin);
            var licensePlate = new LicensePlate("TEST123");

            // Crear el vehículo
            var vehicle = new VehicleEntity(new VehicleId(vehicleId), specs, licensePlate);

            // Establecer el estado según se requiera
            if (!isAvailable)
            {
                vehicle.MarkAsRented();
            }

            return vehicle;
        }
    }
}
