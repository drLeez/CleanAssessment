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
        private readonly IUnitOfWork<int> _unitOfWork;
        public UpdateCustomerHandler(IUnitOfWork<int> unitOfWork)
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

                if (string.IsNullOrEmpty(request.Customer.FirstName) || request.Customer.FirstName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid First Name: \"{request.Customer.FirstName}\"");
                }
                if (request.Customer.MiddleName != null && request.Customer.MiddleName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Middle Name (max length 100 characters): \"{request.Customer.MiddleName}\"");
                }
                if (string.IsNullOrEmpty(request.Customer.LastName) || request.Customer.LastName.Length > 100)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Last Name: \"{request.Customer.LastName}\"");
                }

                if (request.Customer.Age < 18)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Age (must be 18 or older): \"{request.Customer.Age}\"");
                }

                if (request.Customer.Address != null && request.Customer.Address.Length > 500)
                {
                    return await Result<CustomerResponse>.FailAsync($"Invalid Address (max length 500 characters): \"{request.Customer.Address}\"");
                }

                var customer = _unitOfWork.Repository<DB.Models.Customer>().Entities.SingleOrDefault(
                    x => x.CustomerId == request.Customer.CustomerId
                );
                if (customer == null)
                {
                    return await Result<CustomerResponse>.FailAsync($"Could not find Customer to delete: {request.Customer.FullName}");
                }
                
                customer.FirstName = request.Customer.FirstName;
                customer.MiddleName = request.Customer.MiddleName;
                customer.LastName = request.Customer.LastName;
                customer.Age = request.Customer.Age;
                customer.Address = request.Customer.Address;

                await _unitOfWork.Commit(cancellationToken);

                return await Result<CustomerResponse>.SuccessAsync(request.Customer, $"{request.Customer.FullName} has been deleted");
            }
            catch (Exception ex)
            {
                return await Result<CustomerResponse>.FailAsync(ex.Message);
            }
        }
    }
}
