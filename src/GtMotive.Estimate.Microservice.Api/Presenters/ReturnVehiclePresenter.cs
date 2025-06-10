using VehicleCommands = GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using RentalCommands = GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class ReturnVehiclePresenter : VehicleCommands.IReturnVehicleOutputPort, RentalCommands.IReturnVehicleOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public IActionResult ActionResult => throw new System.NotImplementedException();

        public void Success()
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = new { message = "Vehicle returned successfully" };
        }

        public void NotFound(string message)
        {
            _statusCode = StatusCodes.Status404NotFound;
            _viewModel = new { message };
        }

        public void BadRequest(string message)
        {
            _statusCode = StatusCodes.Status400BadRequest;
            _viewModel = new { message };
        }

        public void InternalServerError(string message)
        {
            _statusCode = StatusCodes.Status500InternalServerError;
            _viewModel = new { message };
        }

        public void ReturnedHandle(string vehicleId)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = new { message = $"Vehicle {vehicleId} returned successfully" };
        }

        public void NotFoundHandle(string message)
        {
            NotFound(message);
        }

        public void NotRentedHandle(string message)
        {
            _statusCode = StatusCodes.Status409Conflict;
            _viewModel = new { message };
        }

        public void StandardHandle(string message)
        {
            InternalServerError(message);
        }

        public dynamic GetViewModel() => _viewModel;
        public int GetStatusCode() => _statusCode;

        public void Success(RentalDto rental)
        {
            throw new System.NotImplementedException();
        }
    }
}
