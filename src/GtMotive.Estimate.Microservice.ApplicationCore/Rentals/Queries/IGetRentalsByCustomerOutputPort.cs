using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Queries
{
    /// <summary>
    /// Output port for get rentals by customer use case.
    /// </summary>
    public interface IGetRentalsByCustomerOutputPort : IPresenter
    {
        void Success(IEnumerable<RentalDto> rentals);
        void InternalServerError(string message);
        void NoRentalsFoundHandle(string message);
        void FoundHandle(IEnumerable<RentalDto> rentals);
        void StandardHandle(string message);
    }
}
