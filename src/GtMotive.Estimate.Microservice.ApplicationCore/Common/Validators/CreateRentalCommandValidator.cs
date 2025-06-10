using FluentValidation;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Common.Validators
{
    public class CreateRentalCommandValidator : AbstractValidator<CreateRentalCommand>
    {
        public CreateRentalCommandValidator()
        {
            RuleFor(x => x.CustomerId)
                .NotEmpty().WithMessage("Customer ID is required");

            RuleFor(x => x.VehicleId)
                .NotEmpty().WithMessage("Vehicle ID is required");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(System.DateTime.UtcNow.Date).WithMessage("Start date cannot be in the past");

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate.AddDays(1).AddSeconds(-1))
                .WithMessage("End date must be at least 1 day after start date");
        }
    }
}
