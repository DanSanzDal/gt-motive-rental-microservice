using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs
{
    public class CustomerDto(
        Guid id,
        string name,
        string email,
        string phoneNumber,
        string licenseNumber,
        DateTime? licenseExpiryDate,
        DateTime createdAt)
    {
        public Guid Id { get; } = id;
        public string Name { get; } = name;
        public string Email { get; } = email;
        public string PhoneNumber { get; } = phoneNumber;
        public string LicenseNumber { get; } = licenseNumber;
        public DateTime? LicenseExpiryDate { get; } = licenseExpiryDate;
        public DateTime CreatedAt { get; } = createdAt;
    }
}
