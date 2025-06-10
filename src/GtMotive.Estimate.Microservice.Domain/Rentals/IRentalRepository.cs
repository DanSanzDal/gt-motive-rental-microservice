using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Rentals
{
    public interface IRentalRepository
    {
        Task<Rental> GetByIdAsync(RentalId id);
        Task<Rental> GetActiveRentalByCustomerAsync(CustomerId customerId);
        Task<Rental> GetActiveRentalByVehicleAsync(VehicleId vehicleId);
        Task<IReadOnlyList<Rental>> GetRentalsByCustomerAsync(CustomerId customerId);
        Task<IReadOnlyList<Rental>> GetActiveRentalsAsync();
        Task<IReadOnlyList<Rental>> GetAllAsync();
        Task<bool> HasActiveRentalAsync(CustomerId customerId);
        Task<bool> HasActiveRentalForVehicleAsync(VehicleId vehicleId);
        Task AddAsync(Rental rental);
        Task UpdateAsync(Rental rental);
        Task DeleteAsync(RentalId id);
        Task<IEnumerable<Rental>> GetByCustomerIdAsync(CustomerId customerId);
    }
}
