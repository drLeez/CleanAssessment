using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CleanAssessment.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BaseApiController<T> : ControllerBase
    {
        private IMediator _mediator;
        private ILogger<T> _logger;

        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();
        protected ILogger<T> Logger => _logger ??= HttpContext.RequestServices.GetService<ILogger<T>>();
    }
}
