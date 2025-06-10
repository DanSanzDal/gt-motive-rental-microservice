using System;

namespace GtMotive.Estimate.Microservice.Domain.Customers.Exceptions
{
    public class CustomerNotEligibleException : Exception
    {
        public CustomerNotEligibleException(string message) : base(message) { }

        public CustomerNotEligibleException(string message, Exception innerException) : base(message, innerException) { }
    }
}
