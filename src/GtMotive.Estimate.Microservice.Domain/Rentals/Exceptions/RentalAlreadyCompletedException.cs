using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions
{
    public class RentalAlreadyCompletedException : Exception
    {
        public RentalAlreadyCompletedException(string message) : base(message) { }

        public RentalAlreadyCompletedException(string message, Exception innerException) : base(message, innerException) { }
    }
}
