using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.Customer.Queries;
using CleanAssessment.Domain.Features.PaymentMethod.Queries;
using Microsoft.AspNetCore.Mvc;

namespace CleanAssessment.Controllers
{
    public class PaymentMethodController : BaseApiController<PaymentMethodController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(int customerId)
        {
            var customers = await Mediator.Send(new GetAllPaymentMethodsQuery(customerId));
            return Ok(customers);
        }
    }
}
