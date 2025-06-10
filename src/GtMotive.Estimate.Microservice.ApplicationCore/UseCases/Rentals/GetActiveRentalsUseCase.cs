using System;
using System.Linq;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Rentals;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Use Case for retrieving all active rentals.
    /// </summary>
    public sealed class GetActiveRentalsUseCase(
        IRentalRepository rentalRepository,
        IGetActiveRentalsOutputPort outputPort) : IUseCase<GetActiveRentalsInput>
    {
        public async Task Execute(GetActiveRentalsInput input)
        {
            try
            {
                var activeRentals = await rentalRepository.GetActiveRentalsAsync();

                if (!activeRentals.Any())
                {
                    outputPort.NoRentalsFoundHandle("No active rentals found");
                    return;
                }

                var rentalDtos = activeRentals.Select(rental => new RentalDto
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
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred while retrieving active rentals: {ex.Message}");
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
