using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.UnitTests.Mocks
{
    public class MockRentalRepository : IRentalRepository
    {
        private readonly List<Rental> _rentals = new List<Rental>();

        public Task<Rental> GetByIdAsync(RentalId id)
        {
            return Task.FromResult(_rentals.Find(r => r.Id.Equals(id)));
        }

        public Task<Rental> GetActiveRentalByCustomerAsync(CustomerId customerId)
        {
            return Task.FromResult(_rentals.Find(r => r.CustomerId.Equals(customerId) && r.IsActive()));
        }

        public Task<Rental> GetActiveRentalByVehicleAsync(VehicleId vehicleId)
        {
            return Task.FromResult(_rentals.Find(r => r.VehicleId.Equals(vehicleId) && r.IsActive()));
        }

        public Task<IReadOnlyList<Rental>> GetRentalsByCustomerAsync(CustomerId customerId)
        {
            return Task.FromResult((IReadOnlyList<Rental>)_rentals
                .Where(r => r.CustomerId.Equals(customerId))
                .ToList());
        }

        public Task<IReadOnlyList<Rental>> GetActiveRentalsAsync()
        {
            return Task.FromResult((IReadOnlyList<Rental>)_rentals
                .Where(r => r.IsActive())
                .ToList());
        }

        public Task<IReadOnlyList<Rental>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<Rental>)[.. _rentals]);
        }

        public Task<bool> HasActiveRentalAsync(CustomerId customerId)
        {
            return Task.FromResult(_rentals.Exists(r => r.CustomerId.Equals(customerId) && r.IsActive()));
        }

        public Task<bool> HasActiveRentalForVehicleAsync(VehicleId vehicleId)
        {
            return Task.FromResult(_rentals.Exists(r => r.VehicleId.Equals(vehicleId) && r.IsActive()));
        }

        public Task AddAsync(Rental rental)
        {
            _rentals.Add(rental);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Rental rental)
        {
            var index = _rentals.FindIndex(r => r.Id.Equals(rental.Id));
            if (index >= 0)
            {
                _rentals[index] = rental;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(RentalId id)
        {
            var rental = _rentals.Find(r => r.Id.Equals(id));
            if (rental != null)
            {
                _rentals.Remove(rental);
            }

            return Task.CompletedTask;
        }

        public Task<IEnumerable<Rental>> GetByCustomerIdAsync(CustomerId customerId)
        {
            return Task.FromResult(_rentals
                .Where(r => r.CustomerId.Equals(customerId))
                .AsEnumerable());
        }

        // Método para ayudar en las pruebas
        public void Clear()
        {
            _rentals.Clear();
        }

        // Método para establecer datos de prueba
        public void SetTestData(IEnumerable<Rental> rentals)
        {
            _rentals.Clear();
            _rentals.AddRange(rentals);
        }
    }
}
