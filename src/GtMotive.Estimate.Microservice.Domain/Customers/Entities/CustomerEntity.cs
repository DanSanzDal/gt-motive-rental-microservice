using System;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Customers.Entities
{
    public sealed class CustomerEntity
    {
        public CustomerId Id { get; private set; }
        public string Name { get; private set; }
        public CustomerEmail Email { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public DrivingLicense DrivingLicense { get; private set; }
        public DateTime CreatedAt { get; private set; }

        public CustomerEntity(
            CustomerId id,
            string name,
            CustomerEmail email,
            PhoneNumber phoneNumber,
            DrivingLicense drivingLicense)
        {
            if (id.ToGuid() == Guid.Empty)
            {
                throw new ArgumentException("Customer ID cannot be empty", nameof(id));
            }

            Id = id;
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Email = email;
            PhoneNumber = phoneNumber;
            DrivingLicense = drivingLicense;
            CreatedAt = DateTime.UtcNow;
        }

        // Constructor privado para EF/MongoDB
        private CustomerEntity()
        {
            Id = CustomerId.New();
            Name = string.Empty;
            Email = new CustomerEmail("temp@temp.com");
            PhoneNumber = new PhoneNumber("+1234567890");
            DrivingLicense = new DrivingLicense("TEMP123", DateTime.Now.AddYears(1));
        }

        public void UpdateInfo(string name, CustomerEmail email, PhoneNumber phoneNumber, DrivingLicense drivingLicense)
        {
            Name = !string.IsNullOrWhiteSpace(name) ? name : throw new ArgumentException("Name cannot be null or empty", nameof(name));
            Email = email;
            PhoneNumber = phoneNumber;
            DrivingLicense = drivingLicense;
        }

        public bool IsEligibleForRental()
        {
            return DrivingLicense.IsValid;
        }

        public override string ToString()
        {
            return $"Customer: {Name} ({Email})";
        }

        public override bool Equals(object? obj)
        {
            return obj is CustomerEntity other && Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}
