using Microsoft.AspNetCore.Mvc;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;
using GtMotive.Estimate.Microservice.Api.DTOs.Responses;
using System.Collections.Generic;
using System.Linq;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    /// <summary>
    /// Presenter for Vehicle operations.
    /// </summary>
    public sealed class VehiclePresenter :
        ICreateVehicleOutputPort,
        IRentVehicleOutputPort,
        IReturnVehicleOutputPort,
        IGetAvailableVehiclesOutputPort
    {
        public IActionResult? Result { get; private set; }

        // IPresenter implementation
        public IActionResult ActionResult => Result ?? new StatusCodeResult(500);

        // ICreateVehicleOutputPort
        public void Success(VehicleDto vehicle)
        {
            CreatedHandle(vehicle);
        }

        public void CreatedHandle(VehicleDto vehicle)
        {
            var response = MapToVehicleResponse(vehicle);
            Result = new CreatedResult($"/api/vehicles/{vehicle.Id}", response);
        }

        public void BadRequest(string message)
        {
            StandardHandle(message);
        }

        public void Conflict(string message)
        {
            ConflictHandle(message);
        }

        public void ConflictHandle(string message)
        {
            Result = new ConflictObjectResult(new { Message = message });
        }

        public void InternalServerError(string message)
        {
            Result = new ObjectResult(new { Message = message })
            {
                StatusCode = 500
            };
        }

        public void StandardHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        // IRentVehicleOutputPort
        public void RentedHandle(string vehicleId)
        {
            Result = new OkObjectResult(new { Message = $"Vehicle {vehicleId} rented successfully" });
        }

        public void UnavailableHandle(string message)
        {
            Result = new ConflictObjectResult(new { Message = message });
        }

        public void NotFoundHandle(string message)
        {
            Result = new NotFoundObjectResult(new { Message = message });
        }

        // IReturnVehicleOutputPort
        public void Success()
        {
            Result = new OkObjectResult(new { Message = "Operation completed successfully" });
        }

        public void ReturnedHandle(string vehicleId)
        {
            Result = new OkObjectResult(new { Message = $"Vehicle {vehicleId} returned successfully" });
        }

        public void NotFound(string message)
        {
            NotFoundHandle(message);
        }

        public void NotRentedHandle(string message)
        {
            Result = new BadRequestObjectResult(new { Message = message });
        }

        // IGetAvailableVehiclesOutputPort
        public void FoundHandle(IEnumerable<VehicleDto> vehicles)
        {
            Success(vehicles);
        }

        public void Success(IEnumerable<VehicleDto> vehicles)
        {
            var response = vehicles.Select(MapToVehicleResponse);
            Result = new OkObjectResult(response);
        }

        public void NoVehiclesFoundHandle(string message)
        {
            Result = new OkObjectResult(new List<VehicleResponse>());
        }

        private static VehicleResponse MapToVehicleResponse(VehicleDto dto)
        {
            return new VehicleResponse
            {
                Id = dto.Id.ToString(),
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                ManufactureDate = dto.ManufactureDate,
                VIN = dto.VIN,
                LicensePlate = dto.LicensePlate,
                Status = dto.Status
            };
        }
    }
}
