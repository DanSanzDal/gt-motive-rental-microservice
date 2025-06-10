using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Input for Rent Vehicle Use Case.
    /// </summary>
    public sealed record RentVehicleInput : IUseCaseInput
    {
        public string VehicleId { get; init; } = string.Empty;
    }
}
