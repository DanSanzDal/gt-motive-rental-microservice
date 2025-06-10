using System;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Entities
{
    public class Rental
    {
        public Rental(
            RentalId id,
            CustomerId customerId,
            VehicleId vehicleId,
            RentalPeriod period)
        {
            Id = id;
            CustomerId = customerId;
            VehicleId = vehicleId;
            Period = period;
            Status = RentalStatus.Active;
            CreatedAt = DateTime.UtcNow;
        }

        // Para Entity Framework
        protected Rental() { }

        public RentalId Id { get; private set; }
        public CustomerId CustomerId { get; private set; }
        public VehicleId VehicleId { get; private set; }
        public RentalPeriod Period { get; private set; }
        public RentalStatus Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? CompletedAt { get; private set; }

        public void Complete()
        {
            if (Status != RentalStatus.Active)
            {
                throw new InvalidOperationException($"Rental {Id} is not active");
            }

            Status = RentalStatus.Completed;
            CompletedAt = DateTime.UtcNow;
        }

        public void Cancel()
        {
            if (Status != RentalStatus.Active)
            {
                throw new InvalidOperationException($"Rental {Id} is not active");
            }

            Status = RentalStatus.Cancelled;
            CompletedAt = DateTime.UtcNow;
        }

        public decimal CalculateCost(decimal dailyRate)
        {
            if (Status != RentalStatus.Completed)
            {
                throw new InvalidOperationException("Rental must be completed to calculate cost.");
            }

            var totalDays = Period.GetDurationInDays();
            return totalDays * dailyRate;
        }


        public bool IsActive() => Status == RentalStatus.Active;

        public bool IsOverdue() =>
            Status == RentalStatus.Active && DateTime.UtcNow.Date > Period.EndDate;

        public int GetDurationInDays() => Period.GetDurationInDays();
    }
}
