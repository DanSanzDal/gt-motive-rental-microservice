using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models
{
    public sealed class CustomerModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public string Id { get; set; } = string.Empty;

        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("email")]
        public string Email { get; set; } = string.Empty;

        [BsonElement("phoneNumber")]
        public string PhoneNumber { get; set; } = string.Empty;

        [BsonElement("licenseNumber")]
        public string LicenseNumber { get; set; } = string.Empty;

        [BsonElement("licenseExpiryDate")]
        public DateTime LicenseExpiryDate { get; set; }

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; }

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; }
    }
}
