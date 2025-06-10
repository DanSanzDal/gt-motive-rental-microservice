using System;
using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Input for Create Vehicle Use Case.
    /// </summary>
    public sealed record CreateVehicleInput(
        string VIN,
        string LicensePlate,
        string Brand,
        string Model,
        int Year,
        DateTime ManufactureDate) : IUseCaseInput;
}

