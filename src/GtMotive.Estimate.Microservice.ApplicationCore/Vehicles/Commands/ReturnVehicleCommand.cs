namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    public sealed record ReturnVehicleCommand
    {
        public string VehicleId { get; init; } = string.Empty;
    }
}
