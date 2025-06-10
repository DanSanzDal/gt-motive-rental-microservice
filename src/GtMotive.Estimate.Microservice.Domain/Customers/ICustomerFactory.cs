using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers
{
    public interface ICustomerFactory
    {
        CustomerEntity CreateCustomer(
            string name,
            CustomerEmail email,
            PhoneNumber phoneNumber,
            DrivingLicense drivingLicense);
    }
}
