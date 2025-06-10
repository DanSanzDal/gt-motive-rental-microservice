using FluentValidation;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Validators
{
    public class CreateVehicleCommandValidator : AbstractValidator<CreateVehicleCommand>
    {
        public CreateVehicleCommandValidator()
        {
            RuleFor(x => x.VIN)
                .NotEmpty().WithMessage("VIN is required")
                .Length(17).WithMessage("VIN must be exactly 17 characters")
                .Matches(@"^[A-HJ-NPR-Z0-9]{17}$").WithMessage("VIN contains invalid characters");

            RuleFor(x => x.LicensePlate)
                .NotEmpty().WithMessage("License plate is required")
                .Matches(@"^[0-9]{4}[A-Z]{3}$").WithMessage("License plate must follow format 1234ABC");

            RuleFor(x => x.Brand)
                .NotEmpty().WithMessage("Brand is required")
                .MaximumLength(50).WithMessage("Brand cannot exceed 50 characters");

            RuleFor(x => x.Model)
                .NotEmpty().WithMessage("Model is required")
                .MaximumLength(50).WithMessage("Model cannot exceed 50 characters");

            RuleFor(x => x.ManufactureDate)
                .LessThanOrEqualTo(System.DateTime.UtcNow).WithMessage("Manufacture date cannot be in the future")
                .GreaterThan(System.DateTime.UtcNow.AddYears(-5)).WithMessage("Vehicle cannot be older than 5 years");
        }
    }
}
