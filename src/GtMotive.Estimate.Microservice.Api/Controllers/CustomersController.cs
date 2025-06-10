using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using GtMotive.Estimate.Microservice.Api.Presenters;
using Microsoft.Extensions.DependencyInjection;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.Services;

namespace GtMotive.Estimate.Microservice.Api.Controllers
{
    /// <summary>
    /// Controller for customer management operations.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class CustomersController(CreateCustomerUseCase createCustomerUseCase) : ControllerBase
    {
        /// <summary>
        /// Creates a new customer.
        /// </summary>
        /// <param name="request">Customer creation data</param>
        /// <returns>Created customer information</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerRequest request)
        {
            var presenter = new CustomerPresenter();

            var useCase = new CreateCustomerUseCase(
                HttpContext.RequestServices.GetRequiredService<ICustomerRepository>(),
                HttpContext.RequestServices.GetRequiredService<CustomerService>(),
                presenter);

            // Crear el input usando el constructor con todos los parámetros requeridos
            var input = new CreateCustomerInput(
                Name: request.Name,
                Email: request.Email,
                PhoneNumber: request.PhoneNumber ?? string.Empty,
                LicenseNumber: request.LicenseNumber ?? string.Empty,
                LicenseExpiryDate: request.LicenseExpiryDate
            );

            await useCase.Execute(input);

            return presenter.Result ?? new StatusCodeResult(StatusCodes.Status500InternalServerError);
        }
    }
}
