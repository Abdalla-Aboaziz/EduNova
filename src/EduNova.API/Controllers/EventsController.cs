using EduNova.API.Common;
using EduNova.Application.Features.Events.Commands;
using EduNova.Application.Features.Events.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EventsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("today")]
        public async Task<IActionResult> Today(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsQuery(EventsRange.Today), cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpGet("week")]
        public async Task<IActionResult> Week(CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetEventsQuery(EventsRange.Week), cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpPost("{eventId:guid}/reminder")]
        public async Task<IActionResult> SetReminder(
            [FromRoute] Guid eventId,
            [FromBody] SetReminderCommand command,
            CancellationToken cancellationToken)
        {
            command.EventId = eventId;
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }

        [HttpDelete("{eventId:guid}/reminder")]
        public async Task<IActionResult> CancelReminder(
            [FromRoute] Guid eventId,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new CancelReminderCommand(eventId), cancellationToken);
            return result.ToActionResult(this);
        }
    }
}
