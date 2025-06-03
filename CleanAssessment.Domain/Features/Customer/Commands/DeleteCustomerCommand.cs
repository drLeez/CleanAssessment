using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Domain.Features.Customer.Queries;
using CleanAssessment.Shared.Bases;
using CleanAssessment.Shared.Tools;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.Customer.Commands
{
    public class DeleteCustomerCommand : IRequest<Result<int>>
    {
        public CustomerResponse Customer { get; set; }
        public DeleteCustomerCommand(CustomerResponse customer)
        {
            Customer = customer;
        }
    }
    internal class DeleteCustomerHandler : IRequestHandler<DeleteCustomerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(DeleteCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Customer?.CustomerId == null)
                {
                    return await Result<int>.FailAsync($"Invalid request");
                }
                var customer = _unitOfWork.CustomerRepository.Entities.SingleOrDefault(
                    x => x.CustomerId == request.Customer.CustomerId
                );
                if (customer == null)
                {
                    return await Result<int>.FailAsync($"Could not find Customer to delete: {request.Customer.FullName}");
                }
                await _unitOfWork.CustomerRepository.DeleteAsync(customer);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(0, $"{request.Customer.FullName} has been deleted");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
