using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using InfraModels = GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;
using InfraFactories = GtMotive.Estimate.Microservice.Infrastructure.Factories;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Repositories
{
    public sealed class VehicleRepository(IMongoDatabase database) : IVehicleRepository
    {
        private readonly IMongoCollection<InfraModels.VehicleModel> _collection =
            database.GetCollection<InfraModels.VehicleModel>("vehicles");

        public async Task<Vehicle?> GetByIdAsync(VehicleId id)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.Id, id.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.VehicleFactory.ToEntity(model) : null;
        }

        public async Task<Vehicle?> GetByVINAsync(VIN vin)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.VIN, vin.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.VehicleFactory.ToEntity(model) : null;
        }

        public async Task<Vehicle?> GetByLicensePlateAsync(LicensePlate licensePlate)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.LicensePlate, licensePlate.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.VehicleFactory.ToEntity(model) : null;
        }

        public async Task<IReadOnlyList<Vehicle>> GetAvailableVehiclesAsync()
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.Status, VehicleStatus.Available.ToString());
            var models = await _collection.Find(filter).ToListAsync();

            return models.Select(InfraFactories.VehicleFactory.ToEntity).ToList();
        }

        public async Task<IReadOnlyList<Vehicle>> GetAllAsync()
        {
            var models = await _collection.Find(_ => true)
                .SortBy(v => v.Make)
                .ThenBy(v => v.Model)
                .ToListAsync();

            return models.Select(InfraFactories.VehicleFactory.ToEntity).ToList();
        }

        public async Task<bool> ExistsByVINAsync(VIN vin)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.VIN, vin.ToString());
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> ExistsByLicensePlateAsync(LicensePlate licensePlate)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.LicensePlate, licensePlate.ToString());
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            var model = InfraFactories.VehicleFactory.ToModel(vehicle);
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.Id, vehicle.Id.ToString());
            var existingModel = await _collection.Find(filter).FirstOrDefaultAsync();

            if (existingModel != null)
            {
                InfraFactories.VehicleFactory.UpdateModel(existingModel, vehicle);
                await _collection.ReplaceOneAsync(filter, existingModel);
            }
        }

        public async Task DeleteAsync(VehicleId id)
        {
            var filter = Builders<InfraModels.VehicleModel>.Filter.Eq(v => v.Id, id.ToString());
            await _collection.DeleteOneAsync(filter);
        }
    }
}
