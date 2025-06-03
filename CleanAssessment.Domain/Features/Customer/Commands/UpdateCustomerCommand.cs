using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Shared.Bases;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.Customer.Commands
{
    public class UpdateCustomerCommand : IRequest<Result<CustomerResponse>>
    {
        public CustomerResponse Customer { get; set; }
        public UpdateCustomerCommand(CustomerResponse customer)
        {
            Customer = customer;
        }
    }
    internal class UpdateCustomerHandler : IRequestHandler<UpdateCustomerCommand, Result<CustomerResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public UpdateCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<CustomerResponse>> Handle(UpdateCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Customer?.CustomerId == null)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid request");
                }
                var incoming = request.Customer;
                if (string.IsNullOrEmpty(incoming.FirstName) || incoming.FirstName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid First Name: \"{incoming.FirstName}\"");
                }
                if (incoming.MiddleName != null && incoming.MiddleName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Middle Name (max length 100 characters): \"{incoming.MiddleName}\"");
                }
                if (string.IsNullOrEmpty(incoming.LastName) || incoming.LastName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Last Name: \"{incoming.LastName}\"");
                }

                if (incoming.Age < 18)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Age (must be 18 or older): \"{incoming.Age}\"");
                }

                if (incoming.Address != null && incoming.Address.Length > 500)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Address (max length 500 characters): \"{incoming.Address}\"");
                }

                var customer = _unitOfWork.CustomerRepository.Entities.SingleOrDefault(
                    x => x.CustomerId == incoming.CustomerId
                );
                if (customer == null)
                {
                    return await Result<CustomerResponse>.FailAsync($"Could not find Customer to delete: {incoming.FullName}");
                }
                
                customer.FirstName = incoming.FirstName;
                customer.MiddleName = incoming.MiddleName;
                customer.LastName = incoming.LastName;
                customer.Age = incoming.Age;
                customer.Address = incoming.Address;

                await _unitOfWork.Commit(cancellationToken);

                return await Result<CustomerResponse>.SuccessAsync(incoming, $"{incoming.FullName} has been deleted");
            }
            catch (Exception ex)
            {
                return await Result<CustomerResponse>.FailAsync(ex.Message);
            }
        }
    }
}
