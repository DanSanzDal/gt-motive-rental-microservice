using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Output port for create vehicle use case.
    /// </summary>
    public interface ICreateVehicleOutputPort : IPresenter
    {
        void Success(VehicleDto vehicle);
        void BadRequest(string message);
        void Conflict(string message);
        void InternalServerError(string message);
        void CreatedHandle(VehicleDto vehicle);
        void ConflictHandle(string message);
        void StandardHandle(string message);
    }
}
