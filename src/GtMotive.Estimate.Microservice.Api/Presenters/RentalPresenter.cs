using Microsoft.AspNetCore.Mvc;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using GtMotive.Estimate.Microservice.Api.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    /// <summary>
    /// Presenter for Rental operations.
    /// </summary>
    public sealed class RentalPresenter :
        ICreateRentalOutputPort,
        ICompleteRentalOutputPort,
        IGetActiveRentalsOutputPort,
        IGetRentalsByCustomerOutputPort
    {
        public IActionResult? Result { get; private set; }

        // Implementación de IPresenter
        public IActionResult ActionResult => Result ?? new StatusCodeResult(500);

        #region ICreateRentalOutputPort

        public void Success(RentalDto rental)
        {
            CreatedHandle(rental);
        }

        public void CreatedHandle(RentalDto rental)
        {
            var response = MapToRentalResponse(rental);
            Result = new CreatedResult($"/api/rentals/{rental.Id}", response);
        }

        public void NotFound(string message)
        {
            NotFoundHandle(message);
        }

        public void BadRequest(string message)
        {
            StandardHandle(message);
        }

        public void Conflict(string message)
        {
            VehicleUnavailableHandle(message);
        }

        public void VehicleUnavailableHandle(string message)
        {
            Result = new ConflictObjectResult(new { Message = message });
        }

        public void CustomerIneligibleHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        #endregion

        #region ICompleteRentalOutputPort

        public void Success()
        {
            Result = new OkObjectResult(new { Message = "Operation completed successfully" });
        }

        public void CompletedHandle(RentalDto rental)
        {
            var response = MapToRentalResponse(rental);
            Result = new OkObjectResult(response);
        }

        public void AlreadyCompletedHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        #endregion

        #region IGetActiveRentalsOutputPort & IGetRentalsByCustomerOutputPort

        public void Success(IEnumerable<RentalDto> rentals)
        {
            FoundHandle(rentals);
        }

        public void FoundHandle(IEnumerable<RentalDto> rentals)
        {
            var response = rentals.Select(MapToRentalResponse);
            Result = new OkObjectResult(response);
        }

        public void NoRentalsFoundHandle(string message)
        {
            Result = new OkObjectResult(new List<RentalResponse>());
        }

        #endregion

        #region Métodos compartidos

        public void NotFoundHandle(string message)
        {
            Result = new NotFoundObjectResult(new { Message = message });
        }

        public void StandardHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        public void InternalServerError(string message)
        {
            Result = new ObjectResult(new { Message = message })
            {
                StatusCode = 500
            };
        }

        #endregion

        private static RentalResponse MapToRentalResponse(RentalDto dto)
        {
            return new RentalResponse
            {
                Id = dto.Id,
                CustomerId = dto.CustomerId,
                VehicleId = dto.VehicleId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                DailyRate = dto.DailyRate,
                TotalCost = dto.TotalCost,
                Status = dto.Status
            };
        }
    }
}
