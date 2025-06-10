using FluentValidation;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using System;

namespace GtMotive.Estimate.Microservice.Api.Validators
{
    /// <summary>
    /// Validator for CreateVehicleRequest.
    /// </summary>
    public sealed class CreateVehicleRequestValidator : AbstractValidator<CreateVehicleRequest>
    {
        public CreateVehicleRequestValidator()
        {
            RuleFor(x => x.Brand)
                .NotEmpty()
                .WithMessage("Brand is required")
                .Length(1, 50)
                .WithMessage("Brand must be between 1 and 50 characters");

            RuleFor(x => x.Model)
                .NotEmpty()
                .WithMessage("Model is required")
                .Length(1, 50)
                .WithMessage("Model must be between 1 and 50 characters");

            RuleFor(x => x.Year)
                .InclusiveBetween(1900, DateTime.Now.Year + 1)
                .WithMessage($"Year must be between 1900 and {DateTime.Now.Year + 1}");

            RuleFor(x => x.ManufactureDate)
                .NotEmpty()
                .WithMessage("Manufacture date is required")
                .LessThanOrEqualTo(DateTime.Now)
                .WithMessage("Manufacture date cannot be in the future");

            RuleFor(x => x.VIN)
                .NotEmpty()
                .WithMessage("VIN is required")
                .Length(17)
                .WithMessage("VIN must be exactly 17 characters")
                .Matches("^[A-HJ-NPR-Z0-9]{17}$")
                .WithMessage("VIN contains invalid characters");

            RuleFor(x => x.LicensePlate)
                .NotEmpty()
                .WithMessage("License plate is required")
                .Length(1, 20)
                .WithMessage("License plate must be between 1 and 20 characters");
        }
    }
}
