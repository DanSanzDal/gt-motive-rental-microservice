

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Input for GetRentalsByCustomer use case.
    /// </summary>
    public sealed record GetRentalsByCustomerInput(string CustomerId) : IUseCaseInput;
}
