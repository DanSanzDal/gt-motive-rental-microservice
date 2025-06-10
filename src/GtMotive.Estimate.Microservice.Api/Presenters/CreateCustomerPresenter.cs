using GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GtMotive.Estimate.Microservice.Api.Presenters
{
    public class CreateCustomerPresenter : ICreateCustomerOutputPort
    {
        private int _statusCode;
        private object _viewModel;

        public IActionResult ActionResult => throw new System.NotImplementedException();

        public void Success(CustomerDto customer)
        {
            _statusCode = StatusCodes.Status200OK;
            _viewModel = customer;
        }

        public void Conflict(string message)
        {
            _statusCode = StatusCodes.Status409Conflict;
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

        public void CreatedHandle(CustomerDto customer)
        {
            _statusCode = StatusCodes.Status201Created;
            _viewModel = customer;
        }

        public void ConflictHandle(string message)
        {
            Conflict(message);
        }

        public void StandardHandle(string message)
        {
            InternalServerError(message);
        }

        public dynamic GetViewModel() => _viewModel;
        public int GetStatusCode() => _statusCode;
    }
}
