using System;
using System.Threading.Tasks;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.Commands;
using GtMotive.Estimate.Microservice.ApplicationCore.Customers.DTOs;
using GtMotive.Estimate.Microservice.Domain.Customers;
using GtMotive.Estimate.Microservice.Domain.Customers.ValueObjects;
using GtMotive.Estimate.Microservice.Domain.Customers.Services;
using GtMotive.Estimate.Microservice.Domain.Customers.Exceptions;
using GtMotive.Estimate.Microservice.Domain.Common.ValueObjects;

namespace GtMotive.Estimate.Microservice.ApplicationCore.UseCases.Customers
{
    /// <summary>
    /// Use Case for creating a new customer.
    /// </summary>
    public sealed class CreateCustomerUseCase(
        ICustomerRepository customerRepository,
        CustomerService customerService,
        ICreateCustomerOutputPort outputPort) : IUseCase<CreateCustomerInput>
    {
        public async Task Execute(CreateCustomerInput input)
        {
            try
            {
                var email = new CustomerEmail(input.Email);
                var phoneNumber = new PhoneNumber(input.PhoneNumber);
                var drivingLicense = new DrivingLicense(input.LicenseNumber, input.LicenseExpiryDate);

                var customer = await customerService.CreateCustomerAsync(
                    input.Name,
                    email,
                    phoneNumber,
                    drivingLicense);

                var customerDto = new CustomerDto(
                    customer.Id.ToGuid(),
                    customer.Name,
                    customer.Email.ToString(),
                    customer.PhoneNumber.ToString(),
                    customer.DrivingLicense.Value,
                    customer.DrivingLicense.ExpiryDate,
                    customer.CreatedAt);

                outputPort.CreatedHandle(customerDto);
            }
            catch (DuplicatedCustomerException ex)
            {
                outputPort.ConflictHandle(ex.Message);
            }
            catch (ArgumentException ex)
            {
                outputPort.StandardHandle($"Invalid input: {ex.Message}");
            }
            catch (Exception ex)
            {
                outputPort.StandardHandle($"An error occurred: {ex.Message}");
            }
        }
    }
}
