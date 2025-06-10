using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using GtMotive.Estimate.Microservice.Api.Presenters;
using Microsoft.Extensions.DependencyInjection;
using GtMotive.Estimate.Microservice.Domain.Vehicles;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Services;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for vehicle management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class VehiclesController() : ControllerBase
    {
        /// <summary>
        /// Creates a new vehicle.
        /// </summary>
        /// <param name="request">Vehicle creation data</param>
        /// <returns>Created vehicle information</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateVehicle([FromBody] CreateVehicleRequest request)
        {
            var presenter = new VehiclePresenter();

            // Crear nueva instancia del Use Case con el presenter
            var useCase = new CreateVehicleUseCase(
                HttpContext.RequestServices.GetRequiredService<IVehicleRepository>(),
                HttpContext.RequestServices.GetRequiredService<VehicleService>(),
                presenter);

            var input = new CreateVehicleInput(
                Brand: request.Brand,
                Model: request.Model,
                Year: request.Year,
                ManufactureDate: request.ManufactureDate,
                VIN: request.VIN,
                LicensePlate: request.LicensePlate
            );

            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Rents a vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle ID to rent</param>
        /// <returns>Rent operation result</returns>
        [HttpPost("{vehicleId}/rent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> RentVehicle([FromRoute] string vehicleId)
        {
            var presenter = new VehiclePresenter();

            var useCase = new RentVehicleUseCase(
                HttpContext.RequestServices.GetRequiredService<VehicleService>(),
                presenter);

            var input = new RentVehicleInput { VehicleId = vehicleId };
            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Returns a rented vehicle.
        /// </summary>
        /// <param name="vehicleId">Vehicle ID to return</param>
        /// <returns>Return operation result</returns>
        [HttpPost("{vehicleId}/return")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ReturnVehicle([FromRoute] string vehicleId)
        {
            var presenter = new VehiclePresenter();

            var useCase = new ReturnVehicleUseCase(
                HttpContext.RequestServices.GetRequiredService<VehicleService>(),
                presenter);

            var input = new ReturnVehicleInput(vehicleId);
            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }

        /// <summary>
        /// Gets all available vehicles.
        /// </summary>
        /// <returns>List of available vehicles</returns>
        [HttpGet("available")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAvailableVehicles()
        {
            var presenter = new VehiclePresenter();

            var useCase = new GetAvailableVehiclesUseCase(
                HttpContext.RequestServices.GetRequiredService<IVehicleRepository>(),
                presenter);

            var input = new GetAvailableVehiclesInput();
            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
