using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Use Case for creating a new vehicle.
    /// </summary>
    public sealed class CreateVehicleUseCase(
        IVehicleRepository vehicleRepository,
        VehicleService vehicleService,
        ICreateVehicleOutputPort outputPort) : IUseCase<CreateVehicleInput>
    {
        public async Task Execute(CreateVehicleInput input)
        {
            try
            {
                // Crear Value Objects
                var vin = new VIN(input.VIN);
                var licensePlate = new LicensePlate(input.LicensePlate);

                // Usar el servicio de dominio para crear el vehículo
                var vehicle = await vehicleService.CreateVehicleAsync(
                    vin,
                    licensePlate,
                    input.Brand,
                    input.Model,
                    input.Year,
                    input.ManufactureDate);

                // Persistir
                await vehicleRepository.AddAsync(vehicle);

                // Mapear a DTO y notificar éxito
                var vehicleDto = MapToDto(vehicle);
                outputPort.CreatedHandle(vehicleDto);
            }
            catch (DuplicatedVehicleException ex)
            {
                outputPort.ConflictHandle(ex.Message);
            }
            catch (ArgumentException ex)
            {
                outputPort.StandardHandle($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred: {ex.Message}");
            }
        }

        private static VehicleDto MapToDto(VehicleEntity vehicle)
        {
            return new VehicleDto(
                vehicle.Id.ToGuid(),
                vehicle.Specs.VIN.ToString(),
                vehicle.LicensePlate.ToString(),
                vehicle.Specs.Make,
                vehicle.Specs.Model,
                vehicle.Specs.Year,
                vehicle.Specs.ManufactureDate,
                vehicle.Status.ToString(),
                DateTime.UtcNow);
        }
    }
}
