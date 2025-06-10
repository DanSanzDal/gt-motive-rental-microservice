using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions
{
    /// <summary>
    /// Exception thrown when a rental period is invalid.
    /// </summary>
    public class InvalidRentalPeriodException : Exception
    {
        public InvalidRentalPeriodException() : base("The rental period is invalid.")
        {
        }

        public InvalidRentalPeriodException(string message) : base(message)
        {
        }

        public InvalidRentalPeriodException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
