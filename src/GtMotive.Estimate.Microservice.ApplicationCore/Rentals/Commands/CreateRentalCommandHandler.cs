using System;
using System.Threading;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using MediatR;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    public sealed class CreateRentalCommandHandler : IRequestHandler<CreateRentalCommand, IPresenter>
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IRentalService _rentalService;
        private readonly IVehicleService _vehicleService;
        private readonly ICreateRentalOutputPort _outputPort;

        public CreateRentalCommandHandler(
            IRentalRepository rentalRepository,
            IVehicleRepository vehicleRepository,
            ICustomerRepository customerRepository,
            IRentalService rentalService,
            IVehicleService vehicleService,
            ICreateRentalOutputPort outputPort)
        {
            _rentalRepository = rentalRepository;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
            _rentalService = rentalService;
            _vehicleService = vehicleService;
            _outputPort = outputPort;
        }

        public async Task<IPresenter> Handle(CreateRentalCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var customerId = new CustomerId(request.CustomerId);
                var vehicleId = new VehicleId(request.VehicleId);
                var period = new RentalPeriod(request.StartDate, request.EndDate);

                // Verificar que el cliente existe
                var customer = await _customerRepository.GetByIdAsync(customerId);
                if (customer == null)
                {
                    _outputPort.NotFound($"Customer with ID {customerId} not found");
                    return _outputPort;
                }

                // Verificar que el vehículo existe y está disponible
                var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
                if (vehicle == null)
                {
                    _outputPort.NotFound($"Vehicle with ID {vehicleId} not found");
                    return _outputPort;
                }

                if (!vehicle.IsAvailable())
                {
                    _outputPort.BadRequest($"Vehicle {vehicleId} is not available");
                    return _outputPort;
                }

                // Crear el alquiler usando el domain service
                var rental = await _rentalService.CreateRentalAsync(customerId, vehicleId, period);
                await _rentalRepository.AddAsync(rental);

                // Marcar el vehículo como alquilado
                await _vehicleService.RentVehicleAsync(vehicleId);

                // Aseguramos que el DTO incluya los IDs como strings para que sean detectables
                var rentalDto = new RentalDto
                {
                    Id = rental.Id.ToString(),
                    CustomerId = rental.CustomerId.ToString(),
                    VehicleId = rental.VehicleId.ToString(),
                    StartDate = rental.Period.StartDate,
                    EndDate = rental.Period.EndDate,
                    DailyRate = 0m,
                    Status = rental.Status.ToString()
                };

                _outputPort.CreatedHandle(rentalDto);
                return _outputPort;
            }
            catch (ArgumentException ex)
            {
                _outputPort.BadRequest(ex.Message);
                return _outputPort;
            }
            catch (CustomerAlreadyHasActiveRentalException ex)
            {
                _outputPort.Conflict(ex.Message);
                return _outputPort;
            }
            catch (VehicleNotAvailableException ex)
            {
                _outputPort.Conflict(ex.Message);
                return _outputPort;
            }
            catch (Exception ex)
            {
                _outputPort.InternalServerError($"An error occurred: {ex.Message}");
                return _outputPort;
            }
        }
    }
}

