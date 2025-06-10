using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Entities
{
    public sealed class VehicleEntity : Vehicle
    {
        private VehicleEntity() : base()
        {
        }

        public VehicleEntity(VehicleId id, VehicleSpecs specs, LicensePlate licensePlate)
            : base(id, specs, licensePlate)
        {
        }
    }
}
