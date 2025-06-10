using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands
{
    /// <summary>
    /// Output port for create customer use case.
    /// </summary>
    public interface ICreateCustomerOutputPort : IPresenter
    {
        void Success(CustomerDto customer);
        void Conflict(string message);
        void BadRequest(string message);
        void InternalServerError(string message);
        void CreatedHandle(CustomerDto customer);
        void ConflictHandle(string message);
        void StandardHandle(string message);
    }
}
