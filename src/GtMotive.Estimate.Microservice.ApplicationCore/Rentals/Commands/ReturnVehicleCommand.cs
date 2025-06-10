using System;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Command to return a vehicle and complete rental.
    /// </summary>
    public sealed record ReturnVehicleCommand(Guid RentalId) : IRequest<IPresenter>;
}
