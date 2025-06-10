using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Services
{
    public interface IRentalService
    {
        Task<bool> CanCustomerRentVehicle(CustomerId customerId);
        bool IsVehicleEligibleForFleet(VehicleId vehicleId);
        Task ValidateRentalBusinessRules(CustomerId customerId, VehicleId vehicleId);
        Task<Rental> CreateRentalAsync(CustomerId customerId, VehicleId vehicleId, RentalPeriod period);
        Task CompleteRentalAsync(Rental rental);
        Task ReturnVehicleAsync(RentalId rentalId);
    }
}
