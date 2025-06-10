using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers
{
    /// <summary>
    /// Input for CreateCustomer use case.
    /// </summary>
    public sealed record CreateCustomerInput(
        string Name,
        string Email,
        string PhoneNumber,
        string LicenseNumber,
        DateTime LicenseExpiryDate) : IUseCaseInput;
}
