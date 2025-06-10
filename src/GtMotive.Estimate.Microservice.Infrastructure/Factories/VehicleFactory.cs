using System;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using InfraModels = GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;

namespace GtMotive.Estimate.Microservice.Infrastructure.Factories
{
    public static class VehicleFactory
    {
        public static InfraModels.VehicleModel ToModel(Vehicle vehicle)
        {
            return new InfraModels.VehicleModel
            {
                Id = vehicle.Id.ToString(),
                Make = vehicle.Specs.Make,
                Model = vehicle.Specs.Model,
                Year = vehicle.Specs.Year,
                ManufactureDate = vehicle.Specs.ManufactureDate,
                VIN = vehicle.Specs.VIN.ToString(),
                LicensePlate = vehicle.LicensePlate.ToString(),
                Status = vehicle.Status.ToString(),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static VehicleEntity ToEntity(InfraModels.VehicleModel model)
        {
            var vehicleId = new VehicleId(Guid.Parse(model.Id));
            var vin = new VIN(model.VIN);
            var licensePlate = new LicensePlate(model.LicensePlate);
            var specs = new VehicleSpecs(model.Make, model.Model, model.Year, model.ManufactureDate, vin);

            var vehicle = new VehicleEntity(vehicleId, specs, licensePlate);

            var status = Enum.Parse<VehicleStatus>(model.Status);
            SetVehicleStatus(vehicle, status);

            return vehicle;
        }

        public static void UpdateModel(InfraModels.VehicleModel model, Vehicle vehicle)
        {
            model.Make = vehicle.Specs.Make;
            model.Model = vehicle.Specs.Model;
            model.Year = vehicle.Specs.Year;
            model.ManufactureDate = vehicle.Specs.ManufactureDate;
            model.VIN = vehicle.Specs.VIN.ToString();
            model.LicensePlate = vehicle.LicensePlate.ToString();
            model.Status = vehicle.Status.ToString();
            model.UpdatedAt = DateTime.UtcNow;
        }

        private static void SetVehicleStatus(VehicleEntity vehicle, VehicleStatus status)
        {
            switch (status)
            {
                case VehicleStatus.Available:
                    vehicle.MarkAsAvailable();
                    break;
                case VehicleStatus.Rented:
                    vehicle.MarkAsRented();
                    break;
                case VehicleStatus.Maintenance:
                    vehicle.MarkAsInMaintenance();
                    break;
                case VehicleStatus.OutOfService:
                    break;
                default:
                    vehicle.MarkAsAvailable();
                    break;
            }
        }
    }
}
