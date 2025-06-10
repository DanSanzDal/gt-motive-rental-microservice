using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions
{
    public sealed class VehicleNotFoundException(string message) : Exception(message)
    {
    }
}
