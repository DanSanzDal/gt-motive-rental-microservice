using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;
using GtMotive.Estimate.Microservice.Infrastructure.Configuration;
using Microsoft.Extensions.Options;

namespace GtMotive.Estimate.Microservice.Infrastructure
{
    public sealed class MongoDbSeeder(IMongoDatabase database, IOptions<MongoDbSettings> settings)
    {
        private readonly IMongoDatabase _database = database;
        private readonly MongoDbSettings _settings = settings.Value;

        public async Task SeedAsync()
        {
            await SeedVehiclesAsync();
            await SeedCustomersAsync();
            await CreateIndexesAsync();
        }

        private async Task SeedVehiclesAsync()
        {
            var collection = _database.GetCollection<VehicleModel>(_settings.VehiclesCollectionName);

            if (await collection.CountDocumentsAsync(_ => true) > 0)
            {
                return;
            }

            var sampleVehicles = new List<VehicleModel>
            {
                new() {
                    Id = Guid.NewGuid().ToString(),
                    Make = "Toyota",
                    Model = "Corolla",
                    Year = 2023,
                    ManufactureDate = new DateTime(2023, 1, 15),
                    VIN = "1HGBH41JXMN109186",
                    LicensePlate = "ABC1234",
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    Make = "Honda",
                    Model = "Civic",
                    Year = 2022,
                    ManufactureDate = new DateTime(2022, 8, 20),
                    VIN = "2HGBH41JXMN109187",
                    LicensePlate = "XYZ5678",
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    Make = "Ford",
                    Model = "Focus",
                    Year = 2024,
                    ManufactureDate = new DateTime(2024, 3, 10),
                    VIN = "3HGBH41JXMN109188",
                    LicensePlate = "DEF9012",
                    Status = "Available",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await collection.InsertManyAsync(sampleVehicles);
        }

        private async Task SeedCustomersAsync()
        {
            var collection = _database.GetCollection<CustomerModel>(_settings.CustomersCollectionName);

            if (await collection.CountDocumentsAsync(_ => true) > 0)
            {
                return;
            }

            var sampleCustomers = new List<CustomerModel>
            {
                new() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "Juan Pérez",
                    Email = "juan.perez@email.com",
                    PhoneNumber = "+34600123456",
                    LicenseNumber = "ES123456789",
                    LicenseExpiryDate = new DateTime(2027, 12, 31),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new() {
                    Id = Guid.NewGuid().ToString(),
                    Name = "María García",
                    Email = "maria.garcia@email.com",
                    PhoneNumber = "+34600987654",
                    LicenseNumber = "ES987654321",
                    LicenseExpiryDate = new DateTime(2026, 6, 15),
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            await collection.InsertManyAsync(sampleCustomers);
        }

        private async Task CreateIndexesAsync()
        {
            // Índices para Vehicles
            var vehiclesCollection = _database.GetCollection<VehicleModel>(_settings.VehiclesCollectionName);

            await vehiclesCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<VehicleModel>(
                    Builders<VehicleModel>.IndexKeys.Ascending(v => v.VIN),
                    new CreateIndexOptions { Unique = true }));

            await vehiclesCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<VehicleModel>(
                    Builders<VehicleModel>.IndexKeys.Ascending(v => v.LicensePlate),
                    new CreateIndexOptions { Unique = true }));

            await vehiclesCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<VehicleModel>(
                    Builders<VehicleModel>.IndexKeys.Ascending(v => v.Status)));

            // Índices para Customers
            var customersCollection = _database.GetCollection<CustomerModel>(_settings.CustomersCollectionName);

            await customersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<CustomerModel>(
                    Builders<CustomerModel>.IndexKeys.Ascending(c => c.Email),
                    new CreateIndexOptions { Unique = true }));

            await customersCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<CustomerModel>(
                    Builders<CustomerModel>.IndexKeys.Ascending(c => c.LicenseNumber),
                    new CreateIndexOptions { Unique = true }));

            // Índices para Rentals
            var rentalsCollection = _database.GetCollection<RentalModel>(_settings.RentalsCollectionName);

            await rentalsCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<RentalModel>(
                    Builders<RentalModel>.IndexKeys.Ascending(r => r.CustomerId)));

            await rentalsCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<RentalModel>(
                    Builders<RentalModel>.IndexKeys.Ascending(r => r.VehicleId)));

            await rentalsCollection.Indexes.CreateOneAsync(
                new CreateIndexModel<RentalModel>(
                    Builders<RentalModel>.IndexKeys.Ascending(r => r.Status)));
        }
    }
}
