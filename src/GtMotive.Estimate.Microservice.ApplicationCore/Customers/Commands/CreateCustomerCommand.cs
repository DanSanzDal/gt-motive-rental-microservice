using System;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands
{
    /// <summary>
    /// Command to create a new customer.
    /// </summary>
    public sealed record CreateCustomerCommand(
        string Name,
        string Email,
        string PhoneNumber,
        string DrivingLicense,
        DateTime DrivingLicenseExpiryDate) : IRequest<IPresenter>;
}
