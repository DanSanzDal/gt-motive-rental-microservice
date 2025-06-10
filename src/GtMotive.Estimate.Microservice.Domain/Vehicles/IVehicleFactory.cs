using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    public interface IVehicleFactory
    {
        Vehicle CreateVehicle(
            VIN vin,
            LicensePlate licensePlate,
            VehicleBrand brand,
            VehicleModel model,
            ManufactureDate manufactureDate);
    }
}
