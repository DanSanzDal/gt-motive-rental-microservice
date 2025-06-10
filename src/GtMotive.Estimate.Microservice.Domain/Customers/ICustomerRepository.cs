using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers
{
    public interface ICustomerRepository
    {
        Task<CustomerEntity?> GetByIdAsync(CustomerId customerId);
        Task<CustomerEntity?> GetByEmailAsync(CustomerEmail email);
        Task<CustomerEntity?> GetByLicenseNumberAsync(string licenseNumber);
        Task<IEnumerable<CustomerEntity>> GetAllAsync();
        Task<bool> ExistsByEmailAsync(CustomerEmail email);
        Task<bool> ExistsByLicenseAsync(string licenseNumber);
        Task AddAsync(CustomerEntity customer);
        Task UpdateAsync(CustomerEntity customer);
        Task DeleteAsync(CustomerId customerId);
    }
}
