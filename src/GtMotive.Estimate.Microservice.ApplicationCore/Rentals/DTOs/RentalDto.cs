using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs
{
    /// <summary>
    /// Rental Data Transfer Object.
    /// </summary>
    public sealed record RentalDto
    {
        public string Id { get; init; } = string.Empty;
        public string CustomerId { get; init; } = string.Empty;
        public string VehicleId { get; init; } = string.Empty;
        public DateTime StartDate { get; init; }
        public DateTime? EndDate { get; init; }
        public decimal DailyRate { get; init; }
        public decimal? TotalCost { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
