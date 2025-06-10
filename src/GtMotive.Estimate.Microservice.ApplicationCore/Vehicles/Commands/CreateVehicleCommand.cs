using System;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Command for creating a new vehicle.
    /// </summary>
    public sealed record CreateVehicleCommand(
        string VIN,
        string LicensePlate,
        string Brand,
        string Model,
        int Year,
        DateTime ManufactureDate) : IRequest<IPresenter>;
}
