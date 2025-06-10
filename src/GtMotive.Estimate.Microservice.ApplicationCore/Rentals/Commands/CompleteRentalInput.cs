using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Input for Complete Rental Use Case.
    /// </summary>
    public sealed record CompleteRentalInput : IUseCaseInput
    {
        public string RentalId { get; init; } = string.Empty;
        public DateTime EndDate { get; init; }
    }
}
