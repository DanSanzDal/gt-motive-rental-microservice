using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    public sealed class CompleteRentalCommandHandler(
        IRentalRepository rentalRepository,
        RentalService rentalService,
        VehicleService vehicleService,
        ICompleteRentalOutputPort outputPort) : IRequestHandler<CompleteRentalCommand, IPresenter>
    {
        public async Task<IPresenter> Handle(CompleteRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var rentalId = new RentalId(request.RentalId);

                // Obtener el alquiler
                var rental = await rentalRepository.GetByIdAsync(rentalId);
                if (rental == null)
                {
                    outputPort.NotFound($"Rental with ID {rentalId} not found");
                    return outputPort;
                }

                // Completar el alquiler usando el domain service
                await rentalService.CompleteRentalAsync(rental);

                // Marcar el vehículo como disponible
                await vehicleService.ReturnVehicleAsync(rental.VehicleId);

                outputPort.Success();
                return outputPort;
            }
            catch (RentalNotFoundException ex)
            {
                outputPort.NotFound(ex.Message);
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
    }
}
