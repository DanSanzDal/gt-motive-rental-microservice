using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;
using InfraFactories = GtMotive.Estimate.Microservice.Infrastructure.Factories;

namespace GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Repositories
{
    public sealed class CustomerRepository(IMongoDatabase database) : ICustomerRepository
    {
        private readonly IMongoCollection<CustomerModel> _collection = database.GetCollection<CustomerModel>("customers");

        public async Task<CustomerEntity?> GetByIdAsync(CustomerId customerId)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.Id, customerId.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.CustomerFactory.ToEntity(model) : null;
        }

        public async Task<CustomerEntity?> GetByEmailAsync(CustomerEmail email)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.Email, email.ToString());
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.CustomerFactory.ToEntity(model) : null;
        }

        public async Task<CustomerEntity?> GetByLicenseNumberAsync(string licenseNumber)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.LicenseNumber, licenseNumber);
            var model = await _collection.Find(filter).FirstOrDefaultAsync();

            return model != null ? InfraFactories.CustomerFactory.ToEntity(model) : null;
        }

        public async Task<IEnumerable<CustomerEntity>> GetAllAsync()
        {
            var models = await _collection.Find(_ => true)
                .SortBy(c => c.Name)
                .ToListAsync();

            return models.Select(InfraFactories.CustomerFactory.ToEntity);
        }

        public async Task<bool> ExistsByEmailAsync(CustomerEmail email)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.Email, email.ToString());
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task<bool> ExistsByLicenseAsync(string licenseNumber)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.LicenseNumber, licenseNumber);
            var count = await _collection.CountDocumentsAsync(filter);
            return count > 0;
        }

        public async Task AddAsync(CustomerEntity customer)
        {
            var model = InfraFactories.CustomerFactory.ToModel(customer);
            await _collection.InsertOneAsync(model);
        }

        public async Task UpdateAsync(CustomerEntity customer)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.Id, customer.Id.ToString());
            var existingModel = await _collection.Find(filter).FirstOrDefaultAsync();

            if (existingModel != null)
            {
                InfraFactories.CustomerFactory.UpdateModel(existingModel, customer);
                await _collection.ReplaceOneAsync(filter, existingModel);
            }
        }

        public async Task DeleteAsync(CustomerId customerId)
        {
            var filter = Builders<CustomerModel>.Filter.Eq(c => c.Id, customerId.ToString());
            await _collection.DeleteOneAsync(filter);
        }
    }
}
