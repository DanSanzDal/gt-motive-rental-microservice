using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Output port for return vehicle use case.
    /// </summary>
    public interface IReturnVehicleOutputPort : IPresenter
    {
        void Success();
        void NotFound(string message);
        void BadRequest(string message);
        void InternalServerError(string message);
        void ReturnedHandle(string vehicleId);
        void NotFoundHandle(string message);
        void NotRentedHandle(string message);
        void StandardHandle(string message);
    }
}
