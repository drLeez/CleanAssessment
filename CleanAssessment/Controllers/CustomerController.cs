using CleanAssessment.Domain.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;
using CleanAssessment.Shared.Extensions;
using CleanAssessment.Domain.Features.Customer;
using CleanAssessment.Domain.Features.Customer.Commands;

namespace CleanAssessment.Controllers
{
    public class CustomerController : BaseApiController<CustomerController>
    {
        [HttpGet]
        public async Task<IActionResult> GetAll(int? startDate, int? endDate, string? firstName, string? lastName)
        {
            var customers = await Mediator.Send(new GetAllCustomersQuery()
            {
                StartDate = startDate,
                EndDate = startDate,
                FirstName = firstName,
                LastName = lastName
            });
            return Ok(customers);
        }
        [HttpPost("delete")]
        public async Task<IActionResult> Delete(CustomerResponse customer)
        {
            var response = await Mediator.Send(new DeleteCustomerCommand(customer));
            return Ok(response);
        }
        [HttpPost("update")]
        public async Task<IActionResult> Update(CustomerResponse customer)
        {
            var response = await Mediator.Send(new UpdateCustomerCommand(customer));
            return Ok(response);
        }
    }
}
