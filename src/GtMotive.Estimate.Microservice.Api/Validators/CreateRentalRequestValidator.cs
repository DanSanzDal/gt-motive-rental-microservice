using FluentValidation;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using System;

namespace GtMotive.Estimate.Microservice.Api.Validators
{
    /// <summary>
    /// Validator for CreateRentalRequest.
    /// </summary>
    public sealed class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
    {
        public CreateRentalRequestValidator()
        {
            // CustomerId es tipo Guid
            RuleFor(x => x.CustomerId)
                .NotEmpty()
                .WithMessage("Customer ID is required");

            // VehicleId es tipo Guid
            RuleFor(x => x.VehicleId)
                .NotEmpty()
                .WithMessage("Vehicle ID is required");

            RuleFor(x => x.StartDate)
                .NotEmpty()
                .WithMessage("Start date is required")
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Start date cannot be in the past");

            /*
            RuleFor(x => x.DailyRate)
                .GreaterThan(0)
                .WithMessage("Daily rate must be greater than 0")
                .LessThanOrEqualTo(10000)
                .WithMessage("Daily rate cannot exceed 10,000");
            */
        }
    }
}
