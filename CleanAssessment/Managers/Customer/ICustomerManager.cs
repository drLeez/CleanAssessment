using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Shared.Bases;

namespace CleanAssessment.Managers.Customer
{
    public interface ICustomerManager : IManager
    {
        public Task<IResult<List<CustomerResponse>>> GetAllAsync(DateTime? startDate, DateTime? endDate, string? firstName, string? endName);
        public Task<IResult<int>> DeleteAsync(CustomerResponse customer);
        public Task<IResult<CustomerResponse>> EditAsync(CustomerResponse customer);
        public Task<IResult<int>> AddAsync(CustomerResponse customer);
    }
}
