namespace GtMotive.Estimate.Microservice.Infrastructure.Configuration
{
    public sealed class MongoDbSettings
    {
        public const string SectionName = "MongoDb";

        public string ConnectionString { get; set; } = "mongodb://localhost:27017";
        public string DatabaseName { get; set; } = "RentalTestDb";
        public string VehiclesCollectionName { get; set; } = "vehicles";
        public string CustomersCollectionName { get; set; } = "customers";
        public string RentalsCollectionName { get; set; } = "rentals";
    }
}
