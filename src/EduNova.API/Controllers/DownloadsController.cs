using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Features.Lecture.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DownloadsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DownloadsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        => Ok(await _mediator.Send(new GetMyDownloadsQuery(), cancellationToken));

        [HttpPost("{lectureId}")]
        public async Task<IActionResult> Register([FromRoute] Guid lectureId, CancellationToken cancellationToken)
            => await _mediator.Send(new RegisterDownloadCommand(lectureId), cancellationToken) ? NoContent() : NotFound();

        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> Remove([FromRoute] Guid lectureId, CancellationToken cancellationToken)
            => await _mediator.Send(new RemoveDownloadCommand(lectureId), cancellationToken) ? NoContent() : NotFound();


    }
}
