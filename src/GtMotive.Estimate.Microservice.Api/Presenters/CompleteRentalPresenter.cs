using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class CompleteRentalPresenter : ICompleteRentalOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public IActionResult ActionResult => throw new System.NotImplementedException();

        public void Success()
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = new { message = "Rental completed successfully" };
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

        public void CompletedHandle(RentalDto rental)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = rental;
        }

        public void NotFoundHandle(string message)
        {
            NotFound(message);
        }

        public void AlreadyCompletedHandle(string message)
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
    }
}
