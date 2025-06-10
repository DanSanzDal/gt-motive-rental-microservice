using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    public sealed class CreateVehicleCommandHandler(
        IVehicleRepository vehicleRepository,
        VehicleService vehicleService,
        ICreateVehicleOutputPort outputPort) : IRequestHandler<CreateVehicleCommand, IPresenter>
    {
        public async Task<IPresenter> Handle(CreateVehicleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var vin = new VIN(request.VIN);
                var licensePlate = new LicensePlate(request.LicensePlate);

                var vehicle = await vehicleService.CreateVehicleAsync(
                    vin,
                    licensePlate,
                    request.Brand,
                    request.Model,
                    request.Year,
                    request.ManufactureDate);

                await vehicleRepository.AddAsync(vehicle);

                var vehicleDto = new VehicleDto(
                    vehicle.Id.ToGuid(),
                    vehicle.Specs.VIN.ToString(),
                    vehicle.LicensePlate.ToString(),
                    vehicle.Specs.Make,
                    vehicle.Specs.Model,
                    vehicle.Specs.Year,
                    vehicle.Specs.ManufactureDate,
                    vehicle.Status.ToString(),
                    DateTime.UtcNow);

                outputPort.Success(vehicleDto);
                return outputPort;
            }
            catch (ArgumentException ex)
            {
                outputPort.BadRequest(ex.Message);
                return outputPort;
            }
            catch (DuplicatedVehicleException ex)
            {
                outputPort.Conflict(ex.Message);
                return outputPort;
            }
            catch (VehicleTooOldException ex)
            {
                outputPort.BadRequest(ex.Message);
                return outputPort;
            }
            catch (VehicleNotAvailableException ex)
            {
                outputPort.Conflict(ex.Message);
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
