using System;

namespace GtMotive.Estimate.Microservice.Domain.Vehicles.Exceptions
{
    public class VehicleNotRentedException : Exception
    {
        public VehicleNotRentedException(string message) : base(message) { }

        public VehicleNotRentedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
