using CleanAssessment.Controllers;
using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.PaymentMethod;
using CleanAssessment.Shared.Bases;

namespace CleanAssessment.Managers.PaymentMethod
{
    public interface IPaymentMethodManager : IManager
    {
        public Task<IResult<List<PaymentMethodResponse>>> GetAllAsync(CustomerResponse customer);
    }
}
