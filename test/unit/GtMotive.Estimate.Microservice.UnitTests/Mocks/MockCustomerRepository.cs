using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.UnitTests.Mocks
{
    public class MockCustomerRepository : ICustomerRepository
    {
        private readonly List<CustomerEntity> _customers = new List<CustomerEntity>();

        public Task<CustomerEntity> GetByIdAsync(CustomerId customerId)
        {
            return Task.FromResult(_customers.Find(c => c.Id.Equals(customerId)));
        }

        public Task<CustomerEntity> GetByEmailAsync(CustomerEmail email)
        {
            return Task.FromResult(_customers.Find(c =>
                c.Email.Equals(email)));
        }

        public Task<CustomerEntity> GetByLicenseNumberAsync(string licenseNumber)
        {
            return Task.FromResult(_customers.Find(c =>
                c.DrivingLicense.Value.Equals(licenseNumber, StringComparison.OrdinalIgnoreCase)));
        }

        public Task<IEnumerable<CustomerEntity>> GetAllAsync()
        {
            return Task.FromResult(_customers.AsEnumerable());
        }

        public Task<bool> ExistsAsync(CustomerId id)
        {
            return Task.FromResult(_customers.Exists(c => c.Id.Equals(id)));
        }

        public Task<bool> ExistsByEmailAsync(CustomerEmail email)
        {
            return Task.FromResult(_customers.Exists(c =>
                c.Email.Equals(email)));
        }

        public Task<bool> ExistsByLicenseAsync(string licenseNumber)
        {
            return Task.FromResult(_customers.Exists(c =>
                c.DrivingLicense.Value.Equals(licenseNumber, StringComparison.OrdinalIgnoreCase)));
        }

        public Task AddAsync(CustomerEntity customer)
        {
            _customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task UpdateAsync(CustomerEntity customer)
        {
            var index = _customers.FindIndex(c => c.Id.Equals(customer.Id));
            if (index >= 0)
            {
                _customers[index] = customer;
            }

            return Task.CompletedTask;
        }

        public Task DeleteAsync(CustomerId customerId)
        {
            var customer = _customers.Find(c => c.Id.Equals(customerId));
            if (customer != null)
            {
                _customers.Remove(customer);
            }

            return Task.CompletedTask;
        }

        // Método para ayudar en las pruebas
        public void Clear()
        {
            _customers.Clear();
        }

        // Método para establecer datos de prueba
        public void SetTestData(IEnumerable<CustomerEntity> customers)
        {
            _customers.Clear();
            _customers.AddRange(customers);
        }
    }
}
