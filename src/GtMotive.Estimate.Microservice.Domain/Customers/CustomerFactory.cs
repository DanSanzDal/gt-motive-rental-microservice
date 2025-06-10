using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers
{
    public class CustomerFactory : ICustomerFactory
    {
        public CustomerEntity CreateCustomer(
            string name,
            CustomerEmail email,
            PhoneNumber phoneNumber,
            DrivingLicense drivingLicense)
        {
            var customerId = CustomerId.New();

            return new CustomerEntity(
                customerId,
                name,
                email,
                phoneNumber,
                drivingLicense);
        }
    }
}
