using System.Collections.Generic;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    /// <summary>
    /// Output port for get available vehicles use case.
    /// </summary>
    public interface IGetAvailableVehiclesOutputPort : IPresenter
    {
        void Success(IEnumerable<VehicleDto> vehicles);
        void InternalServerError(string message);
        void NoVehiclesFoundHandle(string message);
        void FoundHandle(IEnumerable<VehicleDto> vehicles);
        void StandardHandle(string message);
    }
}
