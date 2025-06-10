using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Input for Get Available Vehicles Use Case.
    /// </summary>
    public sealed record GetAvailableVehiclesInput : IUseCaseInput
    {
        // Query sin parámetros para obtener todos los vehículos disponibles
    }
}
