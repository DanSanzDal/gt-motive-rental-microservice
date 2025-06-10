using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;
using InfraFactories = GtMotive.Estimate.Microservice.Infrastructure.Factories;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Repositories
{
    public sealed class RentalRepository(IMongoDatabase database) : IRentalRepository
    {
        private readonly IMongoCollection<RentalModel> _collection =
            database.GetCollection<RentalModel>("rentals");

        public async Task<Rental?> GetByIdAsync(RentalId id)
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.Id, id.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.RentalFactory.ToEntity(model) : null;
        }

        public async Task<IEnumerable<Rental>> GetByCustomerIdAsync(CustomerId customerId)
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.CustomerId, customerId.ToString());
            var models = await _collection.Find(filter)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();

            return models.Select(InfraFactories.RentalFactory.ToEntity);
        }

        public async Task<IReadOnlyList<Rental>> GetRentalsByCustomerAsync(CustomerId customerId)
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.CustomerId, customerId.ToString());
            var models = await _collection.Find(filter)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();

            return models.Select(InfraFactories.RentalFactory.ToEntity).ToList();
        }

        public async Task<IReadOnlyList<Rental>> GetActiveRentalsAsync()
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.Status, RentalStatus.Active.ToString());
            var models = await _collection.Find(filter)
                .SortBy(r => r.StartDate)
                .ToListAsync();

            return models.Select(InfraFactories.RentalFactory.ToEntity).ToList();
        }

        public async Task<IReadOnlyList<Rental>> GetAllAsync()
        {
            var models = await _collection.Find(_ => true)
                .SortByDescending(r => r.CreatedAt)
                .ToListAsync();

            return models.Select(InfraFactories.RentalFactory.ToEntity).ToList();
        }

        public async Task<Rental?> GetActiveRentalByCustomerAsync(CustomerId customerId)
        {
            var filter = Builders<RentalModel>.Filter.And(
                Builders<RentalModel>.Filter.Eq(r => r.CustomerId, customerId.ToString()),
                Builders<RentalModel>.Filter.Eq(r => r.Status, RentalStatus.Active.ToString())
            );

            var model = await _collection.Find(filter).FirstOrDefaultAsync();
            return model != null ? InfraFactories.RentalFactory.ToEntity(model) : null;
        }

        public async Task<Rental?> GetActiveRentalByVehicleAsync(VehicleId vehicleId)
        {
            var filter = Builders<RentalModel>.Filter.And(
                Builders<RentalModel>.Filter.Eq(r => r.VehicleId, vehicleId.ToString()),
                Builders<RentalModel>.Filter.Eq(r => r.Status, RentalStatus.Active.ToString())
            );

            var model = await _collection.Find(filter).FirstOrDefaultAsync();
            return model != null ? InfraFactories.RentalFactory.ToEntity(model) : null;
        }

        public async Task<bool> HasActiveRentalAsync(CustomerId customerId)
        {
            var filter = Builders<RentalModel>.Filter.And(
                Builders<RentalModel>.Filter.Eq(r => r.CustomerId, customerId.ToString()),
                Builders<RentalModel>.Filter.Eq(r => r.Status, RentalStatus.Active.ToString())
            );

            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> HasActiveRentalForVehicleAsync(VehicleId vehicleId)
        {
            var filter = Builders<RentalModel>.Filter.And(
                Builders<RentalModel>.Filter.Eq(r => r.VehicleId, vehicleId.ToString()),
                Builders<RentalModel>.Filter.Eq(r => r.Status, RentalStatus.Active.ToString())
            );

            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task AddAsync(Rental rental)
        {
            var model = InfraFactories.RentalFactory.ToModel(rental);
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(Rental rental)
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.Id, rental.Id.ToString());
            var existingModel = await _collection.Find(filter).FirstOrDefaultAsync();

            if (existingModel != null)
            {
                InfraFactories.RentalFactory.UpdateModel(existingModel, rental);
                await _collection.ReplaceOneAsync(filter, existingModel);
            }
        }

        public async Task DeleteAsync(RentalId id)
        {
            var filter = Builders<RentalModel>.Filter.Eq(r => r.Id, id.ToString());
            await _collection.DeleteOneAsync(filter);
        }
    }
}
