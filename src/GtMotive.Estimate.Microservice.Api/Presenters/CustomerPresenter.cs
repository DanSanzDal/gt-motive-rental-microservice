using Microsoft.AspNetCore.Mvc;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;
using GtMotive.Estimate.Microservice.Api.DTOs.Responses;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    /// <summary>
    /// Presenter for Customer operations.
    /// </summary>
    public sealed class CustomerPresenter : ICreateCustomerOutputPort
    {
        public IActionResult? Result { get; private set; }

        // Implementación de IPresenter
        public IActionResult ActionResult => Result ?? new StatusCodeResult(500);

        // Implementación de métodos con nombres genéricos
        public void Success(CustomerDto customer)
        {
            CreatedHandle(customer);
        }

        public void BadRequest(string message)
        {
            StandardHandle(message);
        }

        public void Conflict(string message)
        {
            ConflictHandle(message);
        }

        public void InternalServerError(string message)
        {
            Result = new ObjectResult(new { Message = message })
            {
                StatusCode = 500
            };
        }

        // Implementación de métodos con el sufijo "Handle"
        public void CreatedHandle(CustomerDto customer)
        {
            var response = MapToCustomerResponse(customer);
            Result = new CreatedResult($"/api/customers/{customer.Id}", response);
        }

        public void ConflictHandle(string message)
        {
            Result = new ConflictObjectResult(new { Message = message });
        }

        public void NotFoundHandle(string message)
        {
            Result = new NotFoundObjectResult(new { Message = message });
        }

        public void StandardHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        private static CustomerResponse MapToCustomerResponse(CustomerDto dto)
        {
            return new CustomerResponse
            {
                Id = dto.Id.ToString(),
                Name = dto.Name,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                LicenseNumber = dto.LicenseNumber,
                LicenseExpiryDate = dto.LicenseExpiryDate
            };
        }
    }
}
