using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    public interface IRentalFactory
    {
        Rental CreateRental(
            CustomerId customerId,
            VehicleId vehicleId,
            RentalPeriod period);
    }
}
