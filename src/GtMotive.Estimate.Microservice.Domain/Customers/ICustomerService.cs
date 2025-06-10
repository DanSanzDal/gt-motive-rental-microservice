using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers.Services
{
    public interface ICustomerService
    {
        Task<CustomerEntity> CreateCustomerAsync(
            string name,
            CustomerEmail email,
            PhoneNumber phoneNumber,
            DrivingLicense drivingLicense);

        Task<bool> IsEligibleForRentalAsync(CustomerId customerId);

        Task<CustomerEntity> GetCustomerAsync(CustomerId customerId);
    }
}
