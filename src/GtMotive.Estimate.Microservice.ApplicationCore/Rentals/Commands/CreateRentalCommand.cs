using System;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Command to create a new rental.
    /// </summary>
    public sealed record CreateRentalCommand(
        Guid CustomerId,
        Guid VehicleId,
        DateTime StartDate,
        DateTime EndDate) : IRequest<IPresenter>;
}
