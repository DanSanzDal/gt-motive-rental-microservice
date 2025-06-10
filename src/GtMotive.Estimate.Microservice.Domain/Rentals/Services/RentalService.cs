using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Domain.Vehicles.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Rentals.Entities;
using GtMotive.Estimate.Microservice.Domain.Rentals.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;
        private readonly IVehicleRepository _vehicleRepository;

        public RentalService(
            IRentalRepository rentalRepository,
            IVehicleRepository vehicleRepository)
        {
            _rentalRepository = rentalRepository ?? throw new ArgumentNullException(nameof(rentalRepository));
            _vehicleRepository = vehicleRepository ?? throw new ArgumentNullException(nameof(vehicleRepository));
        }
        public async Task<bool> CanCustomerRentVehicle(CustomerId customerId)
        {
            return !await _rentalRepository.HasActiveRentalAsync(customerId);
        }

        public async Task CompleteRentalAsync(Rental rental)
        {
            // Verificar que el rental existe y está activo
            if (rental == null)
            {
                throw new RentalNotFoundException("Rental cannot be null");
            }

            // Completar el rental usando método del dominio
            rental.Complete();

            // Guardar cambios
            await _rentalRepository.UpdateAsync(rental);
        }

        public async Task<Rental> CreateRentalAsync(CustomerId customerId, VehicleId vehicleId, RentalPeriod period)
        {
            // Validar reglas de negocio antes de crear
            await ValidateRentalBusinessRules(customerId, vehicleId);

            // Generar nuevo ID para el rental
            var rentalId = new RentalId(Guid.NewGuid());

            // Crear el rental usando el constructor de la entidad
            var rental = new Rental(rentalId, customerId, vehicleId, period);

            return rental;
        }

        public bool IsVehicleEligibleForFleet(VehicleId vehicleId)
        {
            // Esta lógica podría expandirse con más reglas
            // Por ejemplo: verificar año, kilometraje, estado, etc.
            return true;
        }

        public async Task ValidateRentalBusinessRules(CustomerId customerId, VehicleId vehicleId)
        {
            // Regla 1: Una persona no puede tener más de 1 vehículo alquilado
            var canRent = await CanCustomerRentVehicle(customerId);
            if (!canRent)
            {
                throw new CustomerAlreadyHasActiveRentalException(
                    $"Customer {customerId} already has an active rental and cannot rent another vehicle");
            }

            // Regla 2: Verificar que el vehículo esté disponible
            var vehicle = await _vehicleRepository.GetByIdAsync(vehicleId) ?? throw new VehicleNotFoundException($"Vehicle {vehicleId} not found");
            if (!vehicle.IsAvailable())
            {
                throw new VehicleNotAvailableException(
                    $"Vehicle {vehicleId} is not available for rental");
            }

            // Regla 3: Verificar que el vehículo sea elegible para la flota
            if (!vehicle.IsEligibleForFleet())
            {
                throw new VehicleTooOldException(
                    $"Vehicle {vehicleId} is too old to be rented");
            }
        }

        public async Task ReturnVehicleAsync(RentalId rentalId)
        {
            // Obtener el alquiler por su ID
            var rental = await _rentalRepository.GetByIdAsync(rentalId) ?? throw new InvalidOperationException("Alquiler no encontrado");

            // Verificar que el alquiler ya no esté completado
            if (rental.Status == RentalStatus.Completed)
            {
                throw new InvalidOperationException("El alquiler ya ha sido completado");
            }

            // Completar el alquiler
            rental.Complete();

            // Hacer que el vehículo esté disponible nuevamente
            var vehicle = await _vehicleRepository.GetByIdAsync(rental.VehicleId);
            vehicle.MarkAsAvailable();

            // Guardar los cambios en el repositorio
            await _rentalRepository.UpdateAsync(rental);
            await _vehicleRepository.UpdateAsync(vehicle);
        }

    }
}
