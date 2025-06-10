using System;

namespace GtMotive.Estimate.Microservice.Api.DTOs.Responses
{
    /// <summary>
    /// Vehicle response DTO.
    /// </summary>
    public sealed record VehicleResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Brand { get; init; } = string.Empty;
        public string Model { get; init; } = string.Empty;
        public int Year { get; init; }
        public DateTime ManufactureDate { get; init; }
        public string VIN { get; init; } = string.Empty;
        public string LicensePlate { get; init; } = string.Empty;
        public string Status { get; init; } = string.Empty;
    }
}
