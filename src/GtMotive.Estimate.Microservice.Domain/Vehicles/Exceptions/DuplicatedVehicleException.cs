using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions
{
    public class DuplicatedVehicleException : Exception
    {
        public DuplicatedVehicleException(string message) : base(message) { }

        public DuplicatedVehicleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
