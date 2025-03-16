using CleanAssessment.Domain.Contracts.Repositories;
using CleanAssessment.Shared.Bases;
using CleanAssessment.Shared.Extensions;
using CleanAssessment.Shared.Tools;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace CleanAssessment.Domain.Features.Customer.Queries
{
    public class GetAllCustomersQuery : IRequest<Result<List<CustomerResponse>>>
    {
        public int? StartDate { get; set; }
        public int? EndDate { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public GetAllCustomersQuery(int? startDate = null, int? endDate = null, string? firstName = null, string? lastName = null)
        {
            StartDate = startDate == 0 ? null : startDate;
            EndDate = endDate == 0 ? null : endDate;
            FirstName = firstName;
            LastName = lastName;
        }
        internal class GetAllCustomersHandler : IRequestHandler<GetAllCustomersQuery, Result<List<CustomerResponse>>>
        {
            private readonly IUnitOfWork<int> _unitOfWork;
            public GetAllCustomersHandler(IUnitOfWork<int> unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task<Result<List<CustomerResponse>>> Handle(GetAllCustomersQuery request, CancellationToken cancellationToken)
            {
                try
                {
                    var validDates =
                        request.StartDate != null
                        && request.StartDate != 0
                        && request.EndDate != null
                        && request.EndDate != 0
                        && request.StartDate < request.EndDate;
                    var validFirstName = !string.IsNullOrEmpty(request.FirstName) && request.FirstName.Length <= 100;
                    var validLastName = !string.IsNullOrEmpty(request.LastName) && request.LastName.Length <= 100;

                    int startDateId = 0, endDateId = 0;
                    if (validDates)
                    {
                        startDateId = request.StartDate.Value;
                        endDateId = request.EndDate.Value;
                    }

                    IQueryable<DB.Models.Customer> raw = _unitOfWork.Repository<DB.Models.Customer>().Entities
                        .OrderByDescending(x => x.CustomerId);
                    if (validDates)
                    {
                        raw = raw.Where(x
                            => x.DOB >= startDateId
                            && x.DOB <= endDateId
                        );
                    }

                    List<CustomerResponse> data = raw.Select(x => new CustomerResponse()
                        {
                            CustomerId = x.CustomerId,
                            FirstName = x.FirstName,
                            MiddleName = x.MiddleName,
                            LastName = x.LastName,
                            NameNumber = x.DuplicateNumber,
                            DateOfBirth = DateTimeTools.FromDateId(x.DOB),
                            Address = x.Address,
                        }).ToList();
                    if (validFirstName)
                    {
                        data = data.Where(
                            x => x.FirstName.Contains(request.FirstName, StringComparison.InvariantCultureIgnoreCase)
                        ).ToList();
                    }
                    if (validLastName)
                    {
                        data = data.Where(
                            x => x.LastName.Contains(request.LastName, StringComparison.InvariantCultureIgnoreCase)
                        ).ToList();
                    }

                    return await Result<List<CustomerResponse>>.SuccessAsync(data);
                }
                catch (Exception ex)
                {
                    return await Result<List<CustomerResponse>>.FailAsync(ex.Message);
                }
            }
        }
    }
}
