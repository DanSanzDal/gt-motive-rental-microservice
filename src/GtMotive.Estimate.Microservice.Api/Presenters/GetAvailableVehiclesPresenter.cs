using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class GetAvailableVehiclesPresenter : IGetAvailableVehiclesOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public IActionResult ActionResult => throw new System.NotImplementedException();

        public void Success(IEnumerable<VehicleDto> vehicles)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = vehicles;
        }

        public void InternalServerError(string message)
        {
            _statusCode = StatusCodes.Status500InternalServerError;
            _viewModel = new { message };
        }

        public void NoVehiclesFoundHandle(string message)
        {
            _statusCode = StatusCodes.Status404NotFound;
            _viewModel = new { message };
        }

        public void FoundHandle(IEnumerable<VehicleDto> vehicles)
        {
            Success(vehicles);
        }

        public void StandardHandle(string message)
        {
            InternalServerError(message);
        }

        public dynamic GetViewModel() => _viewModel;
        public int GetStatusCode() => _statusCode;
    }
}
