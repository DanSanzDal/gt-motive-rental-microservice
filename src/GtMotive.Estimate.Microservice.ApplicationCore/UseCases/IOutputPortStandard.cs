namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases
{
    /// <summary>
    /// Interface to define the Standard Output Port.
    /// </summary>
    public interface IOutputPortStandard
    {
        /// <summary>
        /// Informs the operation was successful.
        /// </summary>
        /// <param name="message">Success message.</param>
        void StandardHandle(string message);
    }
}
