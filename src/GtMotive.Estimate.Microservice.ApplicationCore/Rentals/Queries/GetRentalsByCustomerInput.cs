using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries
{
    /// <summary>
    /// Input for Get Rentals by Customer Use Case.
    /// </summary>
    public sealed record GetRentalsByCustomerInput : IUseCaseInput
    {
        public string CustomerId { get; init; } = string.Empty;
    }
}
