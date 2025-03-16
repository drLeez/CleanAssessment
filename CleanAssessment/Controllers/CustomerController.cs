using CleanAssessment.Domain.Features.Customer.Queries;
using Microsoft.AspNetCore.Mvc;
using CleanAssessment.Shared.Extensions;

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
    }
}
