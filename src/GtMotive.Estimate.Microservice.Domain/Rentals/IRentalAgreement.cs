using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    public interface IRentalAgreement
    {
        RentalAgreementId Id { get; }
        VehicleId VehicleId { get; }
        CustomerId CustomerId { get; }
        RentalPeriod Period { get; }
        RentalStatus Status { get; }
        void Complete(DateTime returnDate);
        void Cancel();
        bool IsActive();
    }
}
