using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Use Case for retrieving rentals by customer.
    /// </summary>
    public sealed class GetRentalsByCustomerUseCase(
        IRentalRepository rentalRepository,
        IGetRentalsByCustomerOutputPort outputPort) : IUseCase<GetRentalsByCustomerInput>
    {
        public async Task Execute(GetRentalsByCustomerInput input)
        {
            try
            {
                var customerId = new CustomerId(Guid.Parse(input.CustomerId));
                var customerRentals = await rentalRepository.GetByCustomerIdAsync(customerId);

                if (!customerRentals.Any())
                {
                    outputPort.NoRentalsFoundHandle($"No rentals found for customer {input.CustomerId}");
                    return;
                }

                var rentalDtos = customerRentals.Select(rental => new RentalDto
                {
                    Id = rental.Id.ToString(),
                    CustomerId = rental.CustomerId.ToString(),
                    VehicleId = rental.VehicleId.ToString(),
                    StartDate = rental.Period.StartDate,
                    EndDate = rental.Period.EndDate,
                    DailyRate = 0m,
                    TotalCost = CalculateTotalCost(rental),
                    Status = rental.Status.ToString()
                }).ToList();

                outputPort.FoundHandle(rentalDtos);
            }
            catch (FormatException)
            {
                outputPort.StandardHandle("Invalid customer ID format");
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred while retrieving customer rentals: {ex.Message}");
            }
        }

        private static decimal? CalculateTotalCost(Domain.Rentals.Entities.Rental rental)
        {
            if (rental.Status != RentalStatus.Completed)
            {
                return null;
            }

            if (!rental.Period.EndDate.HasValue)
            {
                return null;
            }

            var days = (rental.Period.EndDate.Value - rental.Period.StartDate).Days;
            return days > 0 ? days * 50.0m : null; // Ejemplo: 50€ por día
        }
    }
}
