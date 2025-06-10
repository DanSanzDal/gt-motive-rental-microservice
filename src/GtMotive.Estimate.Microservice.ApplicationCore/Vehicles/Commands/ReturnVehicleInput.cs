using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Input for Return Vehicle Use Case.
    /// </summary>
    public sealed record ReturnVehicleInput(string VehicleId) : IUseCaseInput;
}
