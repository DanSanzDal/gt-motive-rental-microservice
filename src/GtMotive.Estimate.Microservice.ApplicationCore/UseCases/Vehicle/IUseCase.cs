using System.Threading.Tasks;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Vehicles
{
    /// <summary>
    /// Base interface for use cases.
    /// </summary>
    public interface IUseCase<in TInput>
    {
        Task Execute(TInput input);
    }
}
