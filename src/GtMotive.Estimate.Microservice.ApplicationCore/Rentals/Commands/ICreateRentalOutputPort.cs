using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Output port for create rental use case.
    /// </summary>
    public interface ICreateRentalOutputPort : IPresenter
    {
        void Success(RentalDto rental);
        void NotFound(string message);
        void BadRequest(string message);
        void Conflict(string message);
        void InternalServerError(string message);
        void CreatedHandle(RentalDto rental);
        void NotFoundHandle(string message);
        void VehicleUnavailableHandle(string message);
        void CustomerIneligibleHandle(string message);
        void StandardHandle(string message);
    }
}
