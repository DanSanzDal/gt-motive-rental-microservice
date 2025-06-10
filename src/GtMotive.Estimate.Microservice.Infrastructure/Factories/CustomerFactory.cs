using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;
using System;

namespace GtMotive.Estimate.Microservice.Infrastructure.Factories
{
    public static class CustomerFactory
    {
        public static CustomerModel ToModel(CustomerEntity customer)
        {
            return new CustomerModel
            {
                Id = customer.Id.ToString(),
                Name = customer.Name,
                Email = customer.Email.ToString(),
                PhoneNumber = customer.PhoneNumber.ToString(),
                LicenseNumber = customer.DrivingLicense.Value,
                LicenseExpiryDate = customer.DrivingLicense.ExpiryDate,
                CreatedAt = customer.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static CustomerEntity ToEntity(CustomerModel model)
        {
            var customerId = new CustomerId(Guid.Parse(model.Id));
            var email = new CustomerEmail(model.Email);
            var phoneNumber = new PhoneNumber(model.PhoneNumber);
            var drivingLicense = new DrivingLicense(model.LicenseNumber, model.LicenseExpiryDate);

            return new CustomerEntity(customerId, model.Name, email, phoneNumber, drivingLicense);
        }

        public static void UpdateModel(CustomerModel model, CustomerEntity customer)
        {
            model.Name = customer.Name;
            model.Email = customer.Email.ToString();
            model.PhoneNumber = customer.PhoneNumber.ToString();
            model.LicenseNumber = customer.DrivingLicense.Value;
            model.LicenseExpiryDate = customer.DrivingLicense.ExpiryDate;
            model.UpdatedAt = DateTime.UtcNow;
        }
    }
}
