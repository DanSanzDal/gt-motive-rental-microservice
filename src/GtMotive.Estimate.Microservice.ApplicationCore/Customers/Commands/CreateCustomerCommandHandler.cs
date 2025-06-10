using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using GtMotive.Estimate.Microservice.ApplicationCore.Common.Interfaces;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands
{
    public sealed class CreateCustomerCommandHandler(
        ICustomerRepository customerRepository,
        ICustomerFactory customerFactory,
        ICreateCustomerOutputPort outputPort) : IRequestHandler<CreateCustomerCommand, IPresenter>
    {
        public async Task<IPresenter> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var email = new CustomerEmail(request.Email);
                var phoneNumber = new PhoneNumber(request.PhoneNumber);
                var drivingLicense = new DrivingLicense(request.DrivingLicense,
                                                      request.DrivingLicenseExpiryDate);

                // Verificar que no existe un cliente con el mismo email
                if (await customerRepository.ExistsByEmailAsync(email))
                {
                    outputPort.Conflict($"Customer with email {email} already exists");
                    return outputPort;
                }

                // Crear cliente usando el factory
                var customer = customerFactory.CreateCustomer(request.Name, email, phoneNumber, drivingLicense);
                await customerRepository.AddAsync(customer);

                var customerDto = new CustomerDto(
                    customer.Id.ToGuid(),
                    customer.Name,
                    customer.Email.ToString(),
                    customer.PhoneNumber.ToString(),
                    customer.DrivingLicense.Value,
                    customer.DrivingLicense.ExpiryDate,
                    customer.CreatedAt);

                outputPort.Success(customerDto);
                return outputPort;
            }
            catch (ArgumentException ex)
            {
                outputPort.BadRequest(ex.Message);
                return outputPort;
            }
            catch (Exception ex)
            {
                outputPort.InternalServerError($"An error occurred: {ex.Message}");
                return outputPort;
            }
        }
    }
}
