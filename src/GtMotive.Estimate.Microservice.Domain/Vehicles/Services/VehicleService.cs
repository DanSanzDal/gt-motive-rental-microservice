using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Services
{
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;

        public VehicleService(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        /// <summary>
        /// Crea un vehículo utilizando objetos de valor de dominio.
        /// </summary>
        public virtual async Task<VehicleEntity> CreateVehicleAsync(
            VIN vin,
            LicensePlate licensePlate,
            string make,
            string model,
            int year,
            DateTime manufactureDate)
        {
            // Verificar que no existe un vehículo con el mismo VIN
            if (await _vehicleRepository.ExistsByVINAsync(vin))
            {
                throw new DuplicatedVehicleException($"Vehicle with VIN {vin} already exists");
            }

            // Verificar que no existe un vehículo con la misma matrícula
            if (await _vehicleRepository.ExistsByLicensePlateAsync(licensePlate))
            {
                throw new DuplicatedVehicleException($"Vehicle with license plate {licensePlate} already exists");
            }

            var vehicleSpecs = new VehicleSpecs(make, model, year, manufactureDate, vin);
            var vehicle = new VehicleEntity(
                VehicleId.New(),
                vehicleSpecs,
                licensePlate);

            return vehicle;
        }

        /// <summary>
        /// Crea un vehículo utilizando valores primitivos. Esta implementación convierte
        /// los valores primitivos en objetos de valor y luego llama al otro método CreateVehicleAsync.
        /// </summary>
        public virtual async Task<VehicleEntity> CreateVehicleAsync(
            string vinValue,
            string licensePlateValue,
            string make,
            string model,
            int year,
            DateTime manufactureDate)
        {
            // Convertir los valores primitivos en objetos de valor
            var vin = new VIN(vinValue);
            var licensePlate = new LicensePlate(licensePlateValue);

            // Delegar al otro método que acepta objetos de valor
            return await CreateVehicleAsync(vin, licensePlate, make, model, year, manufactureDate);
        }

        public virtual async Task RentVehicleAsync(VehicleId vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId)
                ?? throw new VehicleNotFoundException($"Vehicle with ID {vehicleId} not found");

            vehicle.MarkAsRented();
            await _vehicleRepository.UpdateAsync(vehicle);
        }

        public virtual async Task ReturnVehicleAsync(VehicleId vehicleId)
        {
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId)
                ?? throw new VehicleNotFoundException($"Vehicle with ID {vehicleId} not found");

            vehicle.MarkAsAvailable();
            await _vehicleRepository.UpdateAsync(vehicle);
        }
    }
}
