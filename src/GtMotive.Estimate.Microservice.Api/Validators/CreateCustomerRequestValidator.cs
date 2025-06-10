using FluentValidation;
using GtMotive.Estimate.Microservice.Api.DTOs.Requests;
using System;

namespace GtMotive.Estimate.Microservice.Api.Validators
{
    /// <summary>
    /// Validator for CreateCustomerRequest.
    /// </summary>
    public sealed class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
    {
        public CreateCustomerRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Name is required")
                .Length(1, 100)
                .WithMessage("Name must be between 1 and 100 characters");

            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("Email is required")
                .EmailAddress()
                .WithMessage("Email must be a valid email address");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .WithMessage("Phone number is required")
                .Matches(@"^\+?[1-9]\d{1,14}$")
                .WithMessage("Phone number must be in valid international format");

            RuleFor(x => x.LicenseNumber)
                .NotEmpty()
                .WithMessage("License number is required")
                .Length(1, 20)
                .WithMessage("License number must be between 1 and 20 characters");

            RuleFor(x => x.LicenseExpiryDate)
                .NotEmpty()
                .WithMessage("License expiry date is required")
                .GreaterThan(DateTime.Today)
                .WithMessage("License expiry date must be in the future");
        }
    }
}
