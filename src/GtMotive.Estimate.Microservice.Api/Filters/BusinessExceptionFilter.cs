using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Customers.Exceptions;
using System;

namespace GtMotive.Estimate.Microservice.Api.Filters
{
    /// <summary>
    /// Global exception filter for handling business exceptions.
    /// </summary>
    public sealed class BusinessExceptionFilter(ILogger<BusinessExceptionFilter> logger) : IExceptionFilter
    {
        private readonly ILogger<BusinessExceptionFilter> _logger = logger;

        // Define un LoggerMessage estático para mejorar el rendimiento
        private static readonly Action<ILogger, string, Exception> LogException =
            LoggerMessage.Define<string>(
                LogLevel.Error,
                new EventId(1, nameof(OnException)),
                "An exception occurred: {Message}");

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            // Usar el método LoggerMessage definido anteriormente
            LogException(_logger, exception.Message, exception);

            var result = exception switch
            {
                // Vehicle Exceptions
                VehicleNotFoundException => new NotFoundObjectResult(new { exception.Message }),
                VehicleNotAvailableException => new ConflictObjectResult(new { exception.Message }),
                DuplicatedVehicleException => new ConflictObjectResult(new { exception.Message }),
                VehicleNotRentedException => new BadRequestObjectResult(new { exception.Message }),

                // Rental Exceptions
                RentalNotFoundException => new NotFoundObjectResult(new { exception.Message }),
                RentalAlreadyCompletedException => new BadRequestObjectResult(new { exception.Message }),
                InvalidRentalPeriodException => new BadRequestObjectResult(new { exception.Message }),

                // Customer Exceptions
                CustomerNotFoundException => new NotFoundObjectResult(new { exception.Message }),
                DuplicatedCustomerException => new ConflictObjectResult(new { exception.Message }),
                CustomerNotEligibleException => new BadRequestObjectResult(new { exception.Message }),

                // General exceptions
                ArgumentException => new BadRequestObjectResult(new { exception.Message }),
                InvalidOperationException => new BadRequestObjectResult(new { exception.Message }),

                // Unhandled exceptions
                _ => new ObjectResult(new { Message = "An unexpected error occurred" })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                }
            };

            context.Result = result;
            context.ExceptionHandled = true;
        }
    }
}
