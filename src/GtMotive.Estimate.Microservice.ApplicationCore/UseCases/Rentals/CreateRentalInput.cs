using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Input for CreateRental use case.
    /// </summary>
    public sealed record CreateRentalInput(
        string CustomerId,
        string VehicleId,
        DateTime StartDate,
        DateTime EndDate,
        decimal DailyRate) : IUseCaseInput;
}
