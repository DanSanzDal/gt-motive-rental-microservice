using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs
{
    /// <summary>
    /// Vehicle Data Transfer Object.
    /// </summary>
    public sealed record VehicleDto(
        Guid Id,
        string VIN,
        string LicensePlate,
        string Brand,
        string Model,
        int Year,
        DateTime ManufactureDate,
        string Status,
        DateTime CreatedAt);
}
