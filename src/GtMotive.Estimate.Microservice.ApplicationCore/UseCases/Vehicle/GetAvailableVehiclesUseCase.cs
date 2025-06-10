using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Mappings;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Use Case for retrieving all available vehicles.
    /// </summary>
    public sealed class GetAvailableVehiclesUseCase(
        IVehicleRepository vehicleRepository,
        IGetAvailableVehiclesOutputPort outputPort) : IUseCase<GetAvailableVehiclesInput>
    {
        public async Task Execute(GetAvailableVehiclesInput input)
        {
            try
            {
                var vehicles = await vehicleRepository.GetAvailableVehiclesAsync();

                if (!vehicles.Any())
                {
                    outputPort.NoVehiclesFoundHandle("No available vehicles found");
                    return;
                }

                var vehicleDtos = vehicles.Select(vehicle => vehicle.ToDto()).ToList();

                outputPort.FoundHandle(vehicleDtos);
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred while retrieving vehicles: {ex.Message}");
            }
        }
    }
}
