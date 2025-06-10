using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using GtMotive.Estimate.Microservice.Api.Presenters;
using GtMotive.Estimate.Microservice.Domain.Rentals.Services;
using GtMotive.Estimate.Microservice.Domain.Rentals;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Rentals;
using System;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for rental management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class RentalsController(
        IRentalRepository rentalRepository,
        RentalService rentalService) : ControllerBase
    {
        private readonly IRentalRepository _rentalRepository = rentalRepository;
        private readonly RentalService _rentalService = rentalService;

        /// <summary>
        /// Creates a new rental.
        /// </summary>
        /// <param name="request">Rental creation data</param>
        /// <returns>Created rental information</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateRental([FromBody] CreateRentalRequest request)
        {
            var presenter = new RentalPresenter();

            // Crear un nuevo caso de uso con todos los parámetros
            var useCase = new CreateRentalUseCase(
                _rentalRepository,
                _rentalService,
                presenter);

            // Crear la entrada utilizando el constructor en lugar de los inicializadores de objeto
            var input = new CreateRentalInput(
                CustomerId: request.CustomerId.ToString(),
                VehicleId: request.VehicleId.ToString(),
                StartDate: request.StartDate,
                EndDate: request.EndDate,
                DailyRate: 0 // Valor por defecto o request.DailyRate si está disponible
            );

            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Completes a rental.
        /// </summary>
        /// <param name="rentalId">Rental ID to complete</param>
        /// <param name="request">Completion data</param>
        /// <returns>Completed rental information</returns>
        [HttpPost("{rentalId}/complete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> CompleteRental(
            [FromRoute] string rentalId,
            [FromBody] CompleteRentalRequest request)
        {
            var presenter = new RentalPresenter();

            var useCase = new CompleteRentalUseCase(
                _rentalRepository,
                _rentalService,
                presenter);

            // Crear la entrada utilizando el constructor
            var input = new CompleteRentalInput(
                RentalId: rentalId,
                EndDate: request.EndDate
            );

            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Gets all active rentals.
        /// </summary>
        /// <returns>List of active rentals</returns>
        [HttpGet("active")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetActiveRentals()
        {
            var presenter = new RentalPresenter();

            var useCase = new GetActiveRentalsUseCase(
                _rentalRepository,
                presenter);

            var input = new GetActiveRentalsInput();
            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Gets rentals by customer ID.
        /// </summary>
        /// <param name="customerId">Customer ID</param>
        /// <returns>List of customer rentals</returns>
        [HttpGet("customer/{customerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetRentalsByCustomer([FromRoute] string customerId)
        {
            var presenter = new RentalPresenter();

            var useCase = new GetRentalsByCustomerUseCase(
                _rentalRepository,
                presenter);

            // Crear la entrada utilizando el constructor
            var input = new GetRentalsByCustomerInput(customerId);
            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
