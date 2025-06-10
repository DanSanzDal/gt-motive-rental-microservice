using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Use Case for completing a rental.
    /// </summary>
    public sealed class CompleteRentalUseCase(
        IRentalRepository rentalRepository,
        RentalService rentalService,
        ICompleteRentalOutputPort outputPort) : IUseCase<CompleteRentalInput>
    {
        public async Task Execute(CompleteRentalInput input)
        {
            try
            {
                var rentalId = new RentalId(Guid.Parse(input.RentalId));

                var rental = await rentalRepository.GetByIdAsync(rentalId);
                if (rental == null)
                {
                    outputPort.NotFoundHandle($"Rental with ID {rentalId} not found");
                    return;
                }

                if (rental.Status == RentalStatus.Completed)
                {
                    outputPort.AlreadyCompletedHandle($"Rental {rentalId} is already completed");
                    return;
                }

                await rentalService.CompleteRentalAsync(rental);

                var rentalDto = new RentalDto
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

                outputPort.CompletedHandle(rentalDto);
            }
            catch (RentalNotFoundException ex)
            {
                outputPort.NotFoundHandle(ex.Message);
            }
            catch (RentalAlreadyCompletedException ex)
            {
                outputPort.AlreadyCompletedHandle(ex.Message);
            }
            catch (FormatException)
            {
                outputPort.StandardHandle("Invalid rental ID format");
            }
            catch (ArgumentException ex)
            {
                outputPort.StandardHandle($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred: {ex.Message}");
            }
        }
    }
}
