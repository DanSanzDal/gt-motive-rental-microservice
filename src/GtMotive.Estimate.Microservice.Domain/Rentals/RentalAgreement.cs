using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using System;
using GtMotive.Estimate.Microservice.Domain.Common;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    public abstract class RentalAgreement : AggregateRoot<RentalAgreementId>, IRentalAgreement
    {
        protected RentalAgreement()
        {
        }

        protected RentalAgreement(
            RentalAgreementId id,
            VehicleId vehicleId,
            CustomerId customerId,
            DateTime startDate)
        {
            Id = id;
            VehicleId = vehicleId;
            CustomerId = customerId;
            Period = new RentalPeriod(startDate);
            Status = RentalStatus.Active;
        }

        public VehicleId VehicleId { get; protected set; }
        public CustomerId CustomerId { get; protected set; }
        public RentalPeriod Period { get; protected set; }
        public RentalStatus Status { get; protected set; }

        public virtual void Complete(DateTime returnDate)
        {
            if (!IsActive())
            {
                throw new InvalidOperationException($"Rental agreement {Id} is not active and cannot be completed");
            }

            Period = Period.Complete(returnDate);
            Status = RentalStatus.Completed;
        }

        public virtual void Cancel()
        {
            if (!IsActive())
            {
                throw new InvalidOperationException($"Rental agreement {Id} is not active and cannot be cancelled");
            }

            Status = RentalStatus.Cancelled;
        }

        public virtual bool IsActive()
        {
            return Status == RentalStatus.Active;
        }
    }
}
