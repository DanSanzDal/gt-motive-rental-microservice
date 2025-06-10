using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;
using Microsoft.AspNetCore.Http;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class RentVehiclePresenter : IRentVehicleOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public void RentedHandle(string vehicleId)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = new { message = $"Vehicle {vehicleId} rented successfully" };
        }

        public void UnavailableHandle(string message)
        {
            _statusCode = StatusCodes.Status409Conflict;
            _viewModel = new { message };
        }

        public void NotFoundHandle(string message)
        {
            _statusCode = StatusCodes.Status404NotFound;
            _viewModel = new { message };
        }

        public void StandardHandle(string message)
        {
            _statusCode = StatusCodes.Status500InternalServerError;
            _viewModel = new { message };
        }

        public dynamic GetViewModel() => _viewModel;
        public int GetStatusCode() => _statusCode;
    }
}
