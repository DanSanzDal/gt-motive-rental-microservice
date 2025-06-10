using GtMotive.Estimate.Microservice.ApplicationCore.UseCases;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Commands
{
    /// <summary>
    /// Output Port for Rent Vehicle Use Case.
    /// </summary>
    public interface IRentVehicleOutputPort : IOutputPortNotFound, IOutputPortStandard
    {
        /// <summary>
        /// Informs the vehicle was rented successfully.
        /// </summary>
        /// <param name="vehicleId">Rented vehicle ID.</param>
        void RentedHandle(string vehicleId);

        /// <summary>
        /// Informs the vehicle is not available for rental.
        /// </summary>
        /// <param name="message">Unavailable message.</param>
        void UnavailableHandle(string message);
    }
}
