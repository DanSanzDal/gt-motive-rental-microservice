using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class CreateRentalPresenter : ICreateRentalOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public Guid RentalId { get; private set; }

        public IActionResult ActionResult => _statusCode switch
        {
            StatusCodes.Status200OK => new OkObjectResult(_viewModel),
            StatusCodes.Status201Created => new CreatedResult(string.Empty, _viewModel),
            StatusCodes.Status400BadRequest => new BadRequestObjectResult(_viewModel),
            StatusCodes.Status403Forbidden => new ObjectResult(_viewModel) { StatusCode = StatusCodes.Status403Forbidden },
            StatusCodes.Status404NotFound => new NotFoundObjectResult(_viewModel),
            StatusCodes.Status409Conflict => new ConflictObjectResult(_viewModel),
            StatusCodes.Status500InternalServerError => new ObjectResult(_viewModel) { StatusCode = StatusCodes.Status500InternalServerError },
            _ => new OkObjectResult(_viewModel)
        };

        public override string ToString()
        {
            return $"CreateRentalPresenter for Rental {RentalId}";
        }

        public void Success(RentalDto rental)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = rental;
            RentalId = Guid.Parse(rental.Id);
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

        public void Conflict(string message)
        {
            _statusCode = StatusCodes.Status409Conflict;
            _viewModel = new { message };
        }

        public void InternalServerError(string message)
        {
            _statusCode = StatusCodes.Status500InternalServerError;
            _viewModel = new { message };
        }

        public void CreatedHandle(RentalDto rental)
        {
            _statusCode = StatusCodes.Status201Created;
            _viewModel = rental;
            RentalId = Guid.Parse(rental.Id);
        }

        public void NotFoundHandle(string message)
        {
            NotFound(message);
        }

        public void VehicleUnavailableHandle(string message)
        {
            _statusCode = StatusCodes.Status409Conflict;
            _viewModel = new { message };
        }

        public void CustomerIneligibleHandle(string message)
        {
            _statusCode = StatusCodes.Status403Forbidden;
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

