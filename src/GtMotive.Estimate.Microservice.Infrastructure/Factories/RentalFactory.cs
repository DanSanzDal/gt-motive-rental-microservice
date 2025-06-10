using System;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Infrastructure.MongoDB.Models;

namespace GtMotive.Estimate.Microservice.Infrastructure.Factories
{
    public static class RentalFactory
    {
        public static RentalModel ToModel(Rental rental)
        {
            return new RentalModel
            {
                Id = rental.Id.ToString(),
                CustomerId = rental.CustomerId.ToString(),
                VehicleId = rental.VehicleId.ToString(),
                StartDate = rental.Period.StartDate,
                EndDate = rental.Period.EndDate,
                DailyRateAmount = 0m,
                DailyRateCurrency = "EUR",
                TotalCostAmount = null,
                TotalCostCurrency = null,
                Status = rental.Status.ToString(),
                CreatedAt = rental.CreatedAt,
                UpdatedAt = DateTime.UtcNow
            };
        }

        public static Rental ToEntity(RentalModel model)
        {
            var rentalId = new RentalId(Guid.Parse(model.Id));
            var customerId = new CustomerId(Guid.Parse(model.CustomerId));
            var vehicleId = new VehicleId(Guid.Parse(model.VehicleId));
            var period = new RentalPeriod(model.StartDate, model.EndDate);

            return new Rental(rentalId, customerId, vehicleId, period);
        }

        public static void UpdateModel(RentalModel model, Rental rental)
        {
            model.CustomerId = rental.CustomerId.ToString();
            model.VehicleId = rental.VehicleId.ToString();
            model.StartDate = rental.Period.StartDate;
            model.EndDate = rental.Period.EndDate;
            model.DailyRateAmount = 0m;
            model.DailyRateCurrency = "EUR";
            model.TotalCostAmount = null;
            model.TotalCostCurrency = null;
            model.Status = rental.Status.ToString();
            model.UpdatedAt = DateTime.UtcNow;
        }
    }
}
