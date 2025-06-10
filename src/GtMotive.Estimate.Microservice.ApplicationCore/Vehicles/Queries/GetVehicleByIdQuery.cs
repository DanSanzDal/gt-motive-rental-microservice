namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    public sealed record GetVehicleByIdQuery
    {
        public string VehicleId { get; init; } = string.Empty;
    }
}
