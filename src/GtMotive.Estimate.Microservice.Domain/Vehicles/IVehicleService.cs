using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Services
{
    public interface IVehicleService
    {
        Task<VehicleEntity> CreateVehicleAsync(
            VIN vin,
            LicensePlate licensePlate,
            string make,
            string model,
            int year,
            System.DateTime manufactureDate);

        Task RentVehicleAsync(VehicleId vehicleId);

        Task ReturnVehicleAsync(VehicleId vehicleId);
    }
}
