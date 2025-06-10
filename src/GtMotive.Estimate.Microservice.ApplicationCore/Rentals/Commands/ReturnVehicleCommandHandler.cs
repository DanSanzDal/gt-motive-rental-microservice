using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    public sealed class ReturnVehicleCommandHandler(
        IRentalRepository rentalRepository,
        IVehicleRepository vehicleRepository,
        IReturnVehicleOutputPort outputPort) : IRequestHandler<ReturnVehicleCommand, IPresenter>
    {
        public async Task<IPresenter> Handle(ReturnVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rentalId = new RentalId(request.RentalId);

                var rental = await rentalRepository.GetByIdAsync(rentalId);
                if (rental == null)
                {
                    outputPort.NotFound($"Rental with ID {rentalId} not found");
                    return outputPort;
                }

                rental.Complete();
                await rentalRepository.UpdateAsync(rental);

                var vehicle = await vehicleRepository.GetByIdAsync(rental.VehicleId);
                if (vehicle != null)
                {
                    vehicle.MarkAsAvailable();
                    await vehicleRepository.UpdateAsync(vehicle);
                }

                var rentalDto = new RentalDto
                {
                    Id = rental.Id.ToString(),
                    CustomerId = rental.CustomerId.ToString(),
                    VehicleId = rental.VehicleId.ToString(),
                    StartDate = rental.Period.StartDate,
                    EndDate = rental.Period.EndDate,
                    DailyRate = 0m,
                    TotalCost = CalculateTotalCost(rental.Period.StartDate, rental.Period.EndDate),
                    Status = rental.Status.ToString()
                };

                outputPort.Success(rentalDto);
                return outputPort;
            }
            catch (InvalidOperationException ex)
            {
                outputPort.BadRequest(ex.Message);
                return outputPort;
            }
            catch (Exception ex)
            {
                outputPort.InternalServerError($"An error occurred: {ex.Message}");
                return outputPort;
            }
        }

        private static decimal? CalculateTotalCost(DateTime startDate, DateTime? endDate)
        {
            if (!endDate.HasValue)
            {
                return null;
            }

            var days = (endDate.Value - startDate).Days;
            return days > 0 ? days * 50.0m : null;
        }
    }
}
