using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Mappings;
using GtMotive.Estimate.Microservice.Domain.Vehicles;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Vehicles.Queries
{
    public sealed class GetAvailableVehiclesQueryHandler(
        IVehicleRepository vehicleRepository,
        IGetAvailableVehiclesOutputPort outputPort)
        : IRequestHandler<GetAvailableVehiclesQuery, IPresenter>
    {
        public async Task<IPresenter> Handle(GetAvailableVehiclesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var vehicles = await vehicleRepository.GetAvailableVehiclesAsync();
                var vehicleDtos = vehicles.Select(v => v.ToDto()).ToList();

                outputPort.Success(vehicleDtos);
                return outputPort;
            }
            catch (System.Exception ex)
            {
                outputPort.InternalServerError($"An error occurred: {ex.Message}");
                return outputPort;
            }
        }
    }
}
