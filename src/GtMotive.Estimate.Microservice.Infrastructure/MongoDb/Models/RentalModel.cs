using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models
{
    public sealed class RentalModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("customerId")]
        public string CustomerId { get; set; } = string.Empty;

        [BsonElement("vehicleId")]
        public string VehicleId { get; set; } = string.Empty;

        [BsonElement("startDate")]
        public DateTime StartDate { get; set; }

        [BsonElement("endDate")]
        public DateTime? EndDate { get; set; }

        [BsonElement("dailyRateAmount")]
        public decimal DailyRateAmount { get; set; }

        [BsonElement("dailyRateCurrency")]
        public string DailyRateCurrency { get; set; } = string.Empty;

        [BsonElement("totalCostAmount")]
        public decimal? TotalCostAmount { get; set; }

        [BsonElement("totalCostCurrency")]
        public string? TotalCostCurrency { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
