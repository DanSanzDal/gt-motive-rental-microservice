using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries
{
    /// <summary>
    /// Input for Get Active Rentals Use Case.
    /// </summary>
    public sealed record GetActiveRentalsInput : IUseCaseInput
    {
        // Query sin parámetros para obtener todos los alquileres activos
    }
}
