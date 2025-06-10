using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Use Case for returning a rented vehicle.
    /// </summary>
    public sealed class ReturnVehicleUseCase(
        VehicleService vehicleService,
        IReturnVehicleOutputPort outputPort) : IUseCase<ReturnVehicleInput>
    {
        public async Task Execute(ReturnVehicleInput input)
        {
            try
            {
                var vehicleId = new VehicleId(Guid.Parse(input.VehicleId));
                await vehicleService.ReturnVehicleAsync(vehicleId);

                outputPort.ReturnedHandle(input.VehicleId);
            }
            catch (VehicleNotFoundException ex)
            {
                outputPort.NotFoundHandle(ex.Message);
            }
            catch (VehicleNotRentedException ex)
            {
                outputPort.NotRentedHandle(ex.Message);
            }
            catch (FormatException)
            {
                outputPort.StandardHandle("Invalid vehicle ID format");
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred: {ex.Message}");
            }
        }
    }
}
