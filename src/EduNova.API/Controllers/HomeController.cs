using EduNova.API.Common;
using EduNova.Application.Features.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Everything the Home screen needs in one request.</summary>
        [HttpGet("summary")]
        public async Task<IActionResult> Summary(CancellationToken cancellationToken)
            => (await _mediator.Send(new GetHomeSummaryQuery(), cancellationToken)).ToActionResult(this);
    }
}
