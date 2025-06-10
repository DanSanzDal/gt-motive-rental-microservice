using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    public interface IVehicleRepository
    {
        Task<Vehicle> GetByIdAsync(VehicleId id);
        Task<Vehicle> GetByVINAsync(VIN vin);
        Task<Vehicle> GetByLicensePlateAsync(LicensePlate licensePlate);
        Task<IReadOnlyList<Vehicle>> GetAvailableVehiclesAsync();
        Task<IReadOnlyList<Vehicle>> GetAllAsync();
        Task<bool> ExistsByVINAsync(VIN vin);
        Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(VehicleId id);
    }
}
