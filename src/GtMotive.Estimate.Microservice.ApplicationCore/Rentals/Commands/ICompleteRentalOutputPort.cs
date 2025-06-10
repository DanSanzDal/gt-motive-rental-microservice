using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Rentals.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Rentals.Commands
{
    /// <summary>
    /// Output port for complete rental use case.
    /// </summary>
    public interface ICompleteRentalOutputPort : IPresenter
    {
        void Success();
        void NotFound(string message);
        void BadRequest(string message);
        void InternalServerError(string message);
        void CompletedHandle(RentalDto rental);
        void NotFoundHandle(string message);
        void AlreadyCompletedHandle(string message);
        void StandardHandle(string message);
    }
}
