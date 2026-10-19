using EduNova.API.Attributes;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Features.Lecture.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MyListController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MyListController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [RedisCache(duration: 5)]
        public async Task<IActionResult> Get([FromQuery] GetMyListQuery query, CancellationToken cancellationToken)
            => Ok(await _mediator.Send(query, cancellationToken));

        [HttpPost("{lectureId}")]
        public async Task<IActionResult> Add([FromRoute] Guid lectureId, CancellationToken cancellationToken)
            => await _mediator.Send(new AddToMyListCommand(lectureId), cancellationToken) ? NoContent() : NotFound();

        [HttpDelete("{lectureId}")]
        public async Task<IActionResult> Remove([FromRoute] Guid lectureId, CancellationToken cancellationToken)
            => await _mediator.Send(new RemoveFromMyListCommand(lectureId), cancellationToken) ? NoContent() : NotFound();
    }
}
