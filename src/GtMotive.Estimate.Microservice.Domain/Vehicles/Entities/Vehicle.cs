using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Common;
using System;


namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Entities
{
    public abstract class Vehicle : AggregateRoot<VehicleId>, IVehicle
    {
        protected Vehicle()
        {
        }

        protected Vehicle(VehicleId id, VehicleSpecs specs, LicensePlate licensePlate)
        {
            if (!specs.IsEligibleForFleet())
            {
                throw new VehicleTooOldException($"Vehicle manufactured on {specs.ManufactureDate:yyyy-MM-dd} is too old for the fleet");
            }

            Id = id;
            Specs = specs;
            LicensePlate = licensePlate;
            Status = VehicleStatus.Available;
        }

        public VehicleSpecs Specs { get; protected set; }
        public LicensePlate LicensePlate { get; protected set; }
        public VehicleStatus Status { get; protected set; }

        public virtual void MarkAsRented()
        {
            if (!IsAvailable())
            {
                throw new VehicleNotAvailableException($"Vehicle {Id} is not available for rental. Current status: {Status}");
            }

            Status = VehicleStatus.Rented;
        }

        public virtual void MarkAsAvailable()
        {
            Status = VehicleStatus.Available;
        }

        public virtual void MarkAsInMaintenance()
        {
            Status = VehicleStatus.Maintenance;
        }

        public virtual bool IsAvailable()
        {
            return Status == VehicleStatus.Available;
        }

        public virtual bool IsEligibleForFleet()
        {
            return Specs.IsEligibleForFleet();
        }

        public virtual (int TotalYears, int TotalMonths) GetAge()
        {
            var currentDate = DateTime.UtcNow;
            var age = currentDate - Specs.ManufactureDate;

            var years = (int)(age.TotalDays / 365.25);
            var months = (int)(age.TotalDays % 365.25 / 30);

            return (years, months);
        }

        public virtual bool IsOlderThan(TimeSpan timeSpan)
        {
            var currentDate = DateTime.UtcNow;
            var vehicleAge = currentDate - Specs.ManufactureDate;
            return vehicleAge > timeSpan;
        }
    }
}
