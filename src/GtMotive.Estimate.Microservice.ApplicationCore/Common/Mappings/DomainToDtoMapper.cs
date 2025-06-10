using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Entities;
using GtMotive.Estimate.Microservice.Domain.Customers.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Mappings
{
    public static class DomainToDtoMapper
    {
        public static VehicleDto ToDto(this Vehicle vehicle)
        {
            return new VehicleDto(
                vehicle.Id.ToGuid(),
                vehicle.Specs.VIN.ToString(),
                vehicle.LicensePlate.ToString(),
                vehicle.Specs.Make,
                vehicle.Specs.Model,
                vehicle.Specs.Year,
                vehicle.Specs.ManufactureDate,
                vehicle.Status.ToString(),
                DateTime.UtcNow);
        }

        public static CustomerDto ToDto(this CustomerEntity customer)
        {
            return new CustomerDto(
                customer.Id.ToGuid(),
                customer.Name,
                customer.Email.ToString(),
                customer.PhoneNumber.ToString(),
                customer.DrivingLicense.Value,
                customer.DrivingLicense.ExpiryDate,
                DateTime.UtcNow);
        }

        public static RentalDto ToDto(this Rental rental)
        {
            return new RentalDto
            {
                Id = rental.Id.ToString(),
                CustomerId = rental.CustomerId.ToString(),
                VehicleId = rental.VehicleId.ToString(),
                StartDate = rental.Period.StartDate,
                EndDate = rental.Period.EndDate,
                DailyRate = 0m,
                TotalCost = null,
                Status = rental.Status.ToString()
            };
        }
    }
}
