using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Use Case for renting a vehicle.
    /// </summary>
    public sealed class RentVehicleUseCase : IUseCase<RentVehicleInput>
    {
        private readonly VehicleService _vehicleService;
        private readonly IRentVehicleOutputPort _outputPort;

        public RentVehicleUseCase(
            VehicleService vehicleService,
            IRentVehicleOutputPort outputPort)
        {
            _vehicleService = vehicleService;
            _outputPort = outputPort;
        }

        public async Task Execute(RentVehicleInput input)
        {
            try
            {
                var vehicleId = new VehicleId(Guid.Parse(input.VehicleId));
                await _vehicleService.RentVehicleAsync(vehicleId);

                _outputPort.RentedHandle(input.VehicleId);
            }
            catch (VehicleNotFoundException ex)
            {
                _outputPort.NotFoundHandle(ex.Message);
            }
            catch (VehicleNotAvailableException ex)
            {
                _outputPort.UnavailableHandle(ex.Message);
            }
            catch (FormatException)
            {
                _outputPort.StandardHandle("Invalid vehicle ID format");
            }
            catch (Exception ex)
            {
                _outputPort.StandardHandle($"An error occurred: {ex.Message}");
            }
        }
    }
}
