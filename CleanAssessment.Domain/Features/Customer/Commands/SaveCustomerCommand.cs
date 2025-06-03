using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Shared.Bases;
using CleanAssessment.Shared.Extensions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanAssessment.Domain.Features.Customer.Commands
{
    public class SaveCustomerCommand : IRequest<Result<int>>
    {
        public CustomerResponse Customer { get; set; }
        public SaveCustomerCommand(CustomerResponse customer)
        {
            Customer = customer;
        }
    }
    internal class SaveCustomerHandler : IRequestHandler<SaveCustomerCommand, Result<int>>
    {
        private readonly IUnitOfWork _unitOfWork;
        public SaveCustomerHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<int>> Handle(SaveCustomerCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request?.Customer?.CustomerId == null)
                {
                    return await Result<int>.FailAsync($"Invalid request");
                }
                var incoming = request.Customer;
                if (string.IsNullOrEmpty(incoming.FirstName) || incoming.FirstName.Length > 100)
                {
                    return await Result<int>.FailAsync($"Invalid First Name: \"{incoming.FirstName}\"");
                }
                if (incoming.MiddleName != null && incoming.MiddleName.Length > 100)
                {
                    return await Result<int>.FailAsync($"Invalid Middle Name (max length 100 characters): \"{incoming.MiddleName}\"");
                }
                if (string.IsNullOrEmpty(incoming.LastName) || incoming.LastName.Length > 100)
                {
                    return await Result<int>.FailAsync($"Invalid Last Name: \"{incoming.LastName}\"");
                }

                if (incoming.Age < 18)
                {
                    return await Result<int>.FailAsync($"Invalid Age (must be 18 or older): \"{incoming.Age}\"");
                }

                if (incoming.Address != null && incoming.Address.Length > 500)
                {
                    return await Result<int>.FailAsync($"Invalid Address (max length 500 characters): \"{incoming.Address}\"");
                }

                var match = _unitOfWork.CustomerRepository.Entities.Where(
                    x => x.FirstName == incoming.FirstName
                      && x.LastName == incoming.LastName
                      && x.DuplicateNumber == incoming.NameNumber
                ).ToList();
                if (!match.IsNullOrEmpty())
                {
                    return await Result<int>.FailAsync($"Customer with same First/Last Name and # already exist: {incoming.FullName}");
                }

                var customer = new DB.Models.Customer()
                { 
                    FirstName = incoming.FirstName,
                    MiddleName = incoming.MiddleName,
                    LastName = incoming.LastName,
                    DuplicateNumber = incoming.NameNumber,
                    AccDateId = incoming.DateOfAccountCreation.ToDateId(),
                    Age = incoming.Age,
                    Address = incoming.Address,
                };

                var savedCustomer = await _unitOfWork.CustomerRepository.AddAsync(customer);
                await _unitOfWork.Commit(cancellationToken);

                return await Result<int>.SuccessAsync(savedCustomer.CustomerId, $"{incoming.FullName} has been added");
            }
            catch (Exception ex)
            {
                return await Result<int>.FailAsync(ex.Message);
            }
        }
    }
}
