using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers.Services
{
    /// <summary>
    /// Domain service for customer business logic.
    /// </summary>
    public sealed class CustomerService(ICustomerRepository customerRepository) : ICustomerService
    {
        public async Task<CustomerEntity> CreateCustomerAsync(
                string name,
            CustomerEmail email,
            PhoneNumber phoneNumber,
            DrivingLicense drivingLicense)
        {
            // Validar que no exista un cliente con el mismo email
            var existingCustomerByEmail = await customerRepository.GetByEmailAsync(email);
            if (existingCustomerByEmail is not null)
            {
                throw new DuplicatedCustomerException($"Customer with email {email} already exists");
            }

            // Validar que no exista un cliente con el mismo número de licencia
            var existingCustomerByLicense = await customerRepository.GetByLicenseNumberAsync(drivingLicense.Value);
            if (existingCustomerByLicense is not null)
            {
                throw new DuplicatedCustomerException($"Customer with license number {drivingLicense.Value} already exists");
            }

            // Crear y persistir el cliente
            var customerId = CustomerId.New();
            var customer = new CustomerEntity(customerId, name, email, phoneNumber, drivingLicense);

            await customerRepository.AddAsync(customer);

            return customer;
        }

        public async Task<bool> IsEligibleForRentalAsync(CustomerId customerId)
        {
            var customer = await customerRepository.GetByIdAsync(customerId);

            return customer is null
                ? throw new CustomerNotFoundException($"Customer with ID {customerId} not found")
                : customer.IsEligibleForRental();
        }

        public async Task<CustomerEntity> GetCustomerAsync(CustomerId customerId)
        {
            var customer = await customerRepository.GetByIdAsync(customerId);

            return customer ?? throw new CustomerNotFoundException($"Customer with ID {customerId} not found");
        }
    }
}
