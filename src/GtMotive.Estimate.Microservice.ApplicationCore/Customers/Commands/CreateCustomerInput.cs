using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands
{
    /// <summary>
    /// Input for Create Customer Use Case.
    /// </summary>
    public sealed record CreateCustomerInput : IUseCaseInput
    {
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string LicenseNumber { get; init; } = string.Empty;
        public DateTime LicenseExpiryDate { get; init; }
    }
}
