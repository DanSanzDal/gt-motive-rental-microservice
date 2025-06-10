using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models
{
    public sealed class VehicleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("make")]
        public string Make { get; set; } = string.Empty;

        [BsonElement("model")]
        public string Model { get; set; } = string.Empty;

        [BsonElement("year")]
        public int Year { get; set; }

        [BsonElement("manufactureDate")]
        public DateTime ManufactureDate { get; set; }

        [BsonElement("vin")]
        public string VIN { get; set; } = string.Empty;

        [BsonElement("licensePlate")]
        public string LicensePlate { get; set; } = string.Empty;

        [BsonElement("status")]
        public string Status { get; set; } = string.Empty;

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
