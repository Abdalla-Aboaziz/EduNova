using EduNova.API.Common;
using EduNova.Application.Features.Notices.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoticesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoticesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetNoticesQuery(), cancellationToken);
            return result.ToActionResult(this);
        }
    }
}
