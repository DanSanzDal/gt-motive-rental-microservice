using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions
{
    public sealed class VehicleNotAvailableException(string message) : Exception(message)
    {
    }
}
