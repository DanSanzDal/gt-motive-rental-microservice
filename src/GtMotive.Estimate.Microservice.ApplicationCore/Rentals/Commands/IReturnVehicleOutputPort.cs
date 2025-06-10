using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Output port for return vehicle use case.
    /// </summary>
    public interface IReturnVehicleOutputPort : IPresenter
    {
        void Success(RentalDto rental);
        void NotFound(string message);
        void BadRequest(string message);
        void InternalServerError(string message);
    }
}
