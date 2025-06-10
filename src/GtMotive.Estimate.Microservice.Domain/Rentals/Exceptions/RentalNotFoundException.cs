using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions
{
    public class RentalNotFoundException : Exception
    {
        public RentalNotFoundException(string message) : base(message)
        {
        }

        public RentalNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
