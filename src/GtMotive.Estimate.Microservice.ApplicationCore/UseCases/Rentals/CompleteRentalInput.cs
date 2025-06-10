using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Input for CompleteRental use case.
    /// </summary>
    public sealed record CompleteRentalInput(
        string RentalId,
        DateTime EndDate) : IUseCaseInput;
}
