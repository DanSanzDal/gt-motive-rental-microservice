using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Query to get all available vehicles.
    /// </summary>
    public sealed record GetAvailableVehiclesQuery : IRequest<IPresenter>;
}
