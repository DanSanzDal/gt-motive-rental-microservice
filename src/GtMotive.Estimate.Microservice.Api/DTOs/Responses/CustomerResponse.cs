using System;

namespace GtMotive.Estimate.Microservice.Api.DTOs.Responses
{
    /// <summary>
    /// Customer response DTO.
    /// </summary>
    public sealed record CustomerResponse
    {
        public string Id { get; init; } = string.Empty;
        public string Name { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string LicenseNumber { get; init; } = string.Empty;
        public DateTime? LicenseExpiryDate { get; set; }
    }
}
