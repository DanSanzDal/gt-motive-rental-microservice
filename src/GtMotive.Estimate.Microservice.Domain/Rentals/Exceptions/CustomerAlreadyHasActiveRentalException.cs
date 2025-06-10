using System;

namespace GtMotive.Estimate.Microservice.Domain.Rentals.Exceptions
{
    public sealed class CustomerAlreadyHasActiveRentalException(string message) : Exception(message)
    {
    }
}
