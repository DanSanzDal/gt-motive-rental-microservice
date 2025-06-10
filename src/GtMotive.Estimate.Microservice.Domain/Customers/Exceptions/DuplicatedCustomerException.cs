using System;

namespace GtMotive.Estimate.Microservice.Domain.Customers.Exceptions
{
    public class DuplicatedCustomerException : Exception
    {
        public DuplicatedCustomerException(string message) : base(message)
        {
        }

        public DuplicatedCustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
