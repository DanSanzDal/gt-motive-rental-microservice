using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.FunctionalTests.Infrastructure;
using Xunit;

namespace GtMotive.Estimate.Microservice.FunctionalTests.UseCases
{
    [Collection(TestCollections.Functional)]
    public partial class RentVehicleTests(CompositionRootTestFixture fixture) : FunctionalTestBase(fixture)
    {
        [Fact]
        public async Task CreateRentalWithValidDataShouldCreateRental()
        {
            var customer = await GetOrCreateTestCustomer();
            var vehicle = await GetOrCreateAvailableVehicle();

            var startDate = DateTime.UtcNow;
            var endDate = startDate.AddDays(7);

            var command = new CreateRentalCommand(
                customer.Id.ToGuid(),
                vehicle.Id.ToGuid(),
                startDate,
                endDate);

            await Fixture.UsingHandlerForRequestResponse<CreateRentalCommand, IPresenter>(async handler =>
            {
                var presenter = await handler.Handle(command, default);

                Assert.NotNull(presenter);

                Guid rentalGuid = ExtractRentalGuidFromPresenter(presenter, customer.Id.ToString(), vehicle.Id.ToString());
                var rentalId = new RentalId(rentalGuid);

                await Fixture.UsingRepository<IRentalRepository>(async repo =>
                {
                    var rental = await repo.GetByIdAsync(rentalId);

                    Assert.NotNull(rental);
                    Assert.Equal(customer.Id, rental.CustomerId);
                    Assert.Equal(vehicle.Id, rental.VehicleId);
                    Assert.Equal(RentalStatus.Active, rental.Status);
                });

                await Fixture.UsingRepository<IVehicleRepository>(async repo =>
                {
                    var updatedVehicle = await repo.GetByIdAsync(vehicle.Id);
                    Assert.False(updatedVehicle.IsAvailable());
                });
            });
        }

        [System.Text.RegularExpressions.GeneratedRegex(@"[a-fA-F0-9]{8}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{4}-[a-fA-F0-9]{12}")]
        private static partial System.Text.RegularExpressions.Regex MyRegex();

        private static Guid ExtractRentalGuidFromPresenter(object presenter, string expectedCustomerId, string expectedVehicleId)
        {
            if (TryExtractGuidFromProperties(presenter, expectedCustomerId, expectedVehicleId, out Guid rentalGuid))
            {
                return rentalGuid;
            }

            if (TryExtractGuidFromIdProperties(presenter, out rentalGuid))
            {
                return rentalGuid;
            }

            return TryExtractGuidFromString(presenter, out rentalGuid)
                ? rentalGuid
                : throw new InvalidOperationException($"No se pudo extraer el ID del alquiler del presenter: {presenter.GetType().Name}");
        }

        private static bool TryExtractGuidFromProperties(object presenter, string expectedCustomerId, string expectedVehicleId, out Guid result)
        {
            result = Guid.Empty;
            var properties = presenter.GetType().GetProperties();

            foreach (var property in properties)
            {
                var value = property.GetValue(presenter);
                if (value == null)
                {
                    continue;
                }

                if (TryExtractFromRentalDto(value, expectedCustomerId, expectedVehicleId, out result))
                {
                    return true;
                }

                if (TryExtractFromBasicTypes(value, out result))
                {
                    return true;
                }

                if (TryExtractFromCustomDto(value, out result))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool TryExtractFromRentalDto(object value, string expectedCustomerId, string expectedVehicleId, out Guid result)
        {
            result = Guid.Empty;

            if (value is RentalDto dto)
            {
                Assert.Equal(expectedCustomerId, dto.CustomerId);
                Assert.Equal(expectedVehicleId, dto.VehicleId);
                Assert.Equal("Active", dto.Status);

                return Guid.TryParse(dto.Id, out result);
            }

            return false;
        }

        private static bool TryExtractFromBasicTypes(object value, out Guid result)
        {
            result = Guid.Empty;

            if (value is string stringId && Guid.TryParse(stringId, out result))
            {
                return true;
            }

            if (value is Guid guidId)
            {
                result = guidId;
                return true;
            }

            return false;
        }

        private static bool TryExtractFromCustomDto(object value, out Guid result)
        {
            result = Guid.Empty;
            var valueType = value.GetType();

            if (valueType.Name.Contains("Rental", StringComparison.Ordinal) ||
                valueType.Name.Contains("DTO", StringComparison.Ordinal))
            {
                var idProperty = valueType.GetProperty("Id");
                if (idProperty != null)
                {
                    var idValue = idProperty.GetValue(value);
                    return TryExtractFromBasicTypes(idValue, out result);
                }
            }

            return false;
        }

        private static bool TryExtractGuidFromIdProperties(object presenter, out Guid result)
        {
            result = Guid.Empty;
            Type presenterType = presenter.GetType();
            PropertyInfo[] properties = presenterType.GetProperties();

            foreach (var property in properties)
            {
                if (property.Name.Contains("Id", StringComparison.Ordinal) ||
                    property.Name.Contains("ID", StringComparison.Ordinal))
                {
                    var value = property.GetValue(presenter);
                    if (TryExtractFromBasicTypes(value, out result))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static bool TryExtractGuidFromString(object presenter, out Guid result)
        {
            result = Guid.Empty;
            string presenterString = presenter.ToString();
            var guidMatches = MyRegex().Matches(presenterString);

            return guidMatches.Count > 0 && Guid.TryParse(guidMatches[0].Value, out result);
        }

        private async Task<CustomerEntity> GetOrCreateTestCustomer()
        {
            CustomerEntity customer = null;

            await Fixture.UsingRepository<ICustomerRepository>(async repo =>
            {
                var customers = await repo.GetAllAsync();
                customer = customers.FirstOrDefault();

                if (customer == null)
                {
                    var customerId = CustomerId.New();
                    var email = new CustomerEmail("test.customer@example.com");
                    var phoneNumber = new PhoneNumber("+341234567890");
                    var drivingLicense = new DrivingLicense("TEST12345", DateTime.UtcNow.AddYears(1));

                    customer = new CustomerEntity(
                        customerId,
                        "Test Customer",
                        email,
                        phoneNumber,
                        drivingLicense);

                    await repo.AddAsync(customer);
                }
            });

            return customer;
        }

        private async Task<Vehicle> GetOrCreateAvailableVehicle()
        {
            Vehicle vehicle = null;

            await Fixture.UsingRepository<IVehicleRepository>(async repo =>
            {
                var vehicles = await repo.GetAvailableVehiclesAsync();

                if (vehicles.Count > 0)
                {
                    vehicle = vehicles[0];
                }
                else if (vehicles.Any())
                {
                    vehicle = vehicles[0];
                }

                if (vehicle == null)
                {
                    var vehicleId = VehicleId.New();
                    var manufactureDate = DateTime.UtcNow.AddYears(-2);

                    var vin = new VIN("WDB2030461A123456");

                    var specs = new VehicleSpecs(
                        "TestMake",
                        "TestModel",
                        DateTime.UtcNow.Year - 2,
                        manufactureDate,
                        vin);
                    var licensePlate = new LicensePlate("TEST123");

                    vehicle = new VehicleEntity(vehicleId, specs, licensePlate);
                    await repo.AddAsync(vehicle);
                }
            });

            return vehicle;
        }
    }
}
