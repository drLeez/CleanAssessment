using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Shared.Bases;

namespace CleanAssessment.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        public Task<IResult<List<CustomerResponse>>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? firstName, string? endName);
    }
}
