using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using System;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Command to complete a rental.
    /// </summary>
    public sealed record CompleteRentalCommand(Guid RentalId) : IRequest<IPresenter>;
}
