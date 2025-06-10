using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;

namespace GtMotive.Estimate.Microservice.UnitTests.Mocks
{
    public class MockVehicleRepository : IVehicleRepository
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        public Task<Vehicle> GetByIdAsync(VehicleId id)
        {
            return Task.FromResult(_vehicles.Find(v => v.Id.Equals(id)));
        }

        public Task<Vehicle> GetByVINAsync(VIN vin)
        {
            return Task.FromResult(_vehicles.Find(v => v.Specs.VIN.Equals(vin)));
        }

        public Task<Vehicle> GetByLicensePlateAsync(LicensePlate licensePlate)
        {
            return Task.FromResult(_vehicles.Find(v => v.LicensePlate.Equals(licensePlate)));
        }

        public Task<IReadOnlyList<Vehicle>> GetAvailableVehiclesAsync()
        {
            return Task.FromResult((IReadOnlyList<Vehicle>)_vehicles.Where(v => v.IsAvailable()).ToList());
        }

        public Task<IReadOnlyList<Vehicle>> GetAllAsync()
        {
            return Task.FromResult((IReadOnlyList<Vehicle>)[.. _vehicles]);
        }

        public Task<bool> ExistsByVINAsync(VIN vin)
        {
            return Task.FromResult(_vehicles.Exists(v => v.Specs.VIN.Equals(vin)));
        }

        public Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate)
        {
            return Task.FromResult(_vehicles.Exists(v => v.LicensePlate.Equals(licensePlate)));
        }

        public Task AddAsync(Vehicle vehicle)
        {
            _vehicles.Add(vehicle);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(Vehicle vehicle)
        {
            var index = _vehicles.FindIndex(v => v.Id.Equals(vehicle.Id));
            if (index >= 0)
            {
                _vehicles[index] = vehicle;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(VehicleId id)
        {
            var vehicle = _vehicles.Find(v => v.Id.Equals(id));
            if (vehicle != null)
            {
                _vehicles.Remove(vehicle);
            }

            return Task.CompletedTask;
        }

        // Método para ayudar en las pruebas
        public void Clear()
        {
            _vehicles.Clear();
        }

        // Método para establecer datos de prueba
        public void SetTestData(IEnumerable<Vehicle> vehicles)
        {
            _vehicles.Clear();
            _vehicles.AddRange(vehicles);
        }
    }
}
