using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Customers.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals
{
    /// <summary>
    /// Use Case for creating a new rental.
    /// </summary>
    public sealed class CreateRentalUseCase(
        IRentalRepository rentalRepository,
        RentalService rentalService,
        ICreateRentalOutputPort outputPort) : IUseCase<CreateRentalInput>
    {
        public async Task Execute(CreateRentalInput input)
        {
            try
            {
                var customerId = new CustomerId(Guid.Parse(input.CustomerId));
                var vehicleId = new VehicleId(Guid.Parse(input.VehicleId));
                var period = new RentalPeriod(input.StartDate, input.EndDate);

                var rental = await rentalService.CreateRentalAsync(customerId, vehicleId, period);
                await rentalRepository.AddAsync(rental);

                var rentalDto = new RentalDto
                {
                    Id = rental.Id.ToString(),
                    CustomerId = rental.CustomerId.ToString(),
                    VehicleId = rental.VehicleId.ToString(),
                    StartDate = rental.Period.StartDate,
                    EndDate = rental.Period.EndDate,
                    DailyRate = input.DailyRate,
                    TotalCost = CalculateTotalCost(input.StartDate, input.EndDate, input.DailyRate),
                    Status = rental.Status.ToString()
                };

                outputPort.CreatedHandle(rentalDto);
            }
            catch (CustomerNotFoundException ex)
            {
                outputPort.NotFoundHandle($"Customer not found: {ex.Message}");
            }
            catch (VehicleNotFoundException ex)
            {
                outputPort.NotFoundHandle($"Vehicle not found: {ex.Message}");
            }
            catch (VehicleNotAvailableException ex)
            {
                outputPort.VehicleUnavailableHandle(ex.Message);
            }
            catch (CustomerNotEligibleException ex)
            {
                outputPort.CustomerIneligibleHandle(ex.Message);
            }
            catch (FormatException)
            {
                outputPort.StandardHandle("Invalid ID format");
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

        private static decimal? CalculateTotalCost(DateTime startDate, DateTime endDate, decimal dailyRate)
        {
            var days = (endDate - startDate).Days;
            return days > 0 ? days * dailyRate : null;
        }
    }
}
