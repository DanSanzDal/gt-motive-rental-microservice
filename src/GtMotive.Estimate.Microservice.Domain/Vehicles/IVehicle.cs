
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles
{
    public interface IVehicle
    {
        VehicleId Id { get; }
        bool IsAvailable();
        bool IsEligibleForFleet();
        void MarkAsRented();
        void MarkAsAvailable();
    }
}
