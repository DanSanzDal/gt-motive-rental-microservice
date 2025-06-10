using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Input for Create Rental Use Case.
    /// </summary>
    public sealed record CreateRentalInput : IUseCaseInput
    {
        public string CustomerId { get; init; } = string.Empty;
        public string VehicleId { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public decimal DailyRate { get; init; }
    }
}
