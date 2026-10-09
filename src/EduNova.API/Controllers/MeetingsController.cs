using EduNova.API.Attributes;
using EduNova.API.Common;
using EduNova.Application.Features.Meetings.Commands;
using EduNova.Application.Features.Meetings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MeetingsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        #region Create
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMeetingCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            // الـ ToActionResult هترجع 200 OK بالداتا في حالة النجاح، أو Error ProblemDetails في حالة الفشل
            return result.ToActionResult(this);
        }
        #endregion

        #region Get By Id
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(new GetMeetingByIdQuery(id), cancellationToken);
            return result.ToActionResult(this);
        }

        #endregion

        #region Get All
        
        [HttpGet]
        [RedisCache(duration: 5)]
        public async Task<IActionResult> GetAll([FromQuery] GetMeetingsQuery query, CancellationToken cancellationToken)
        {
            // الـ query هنا هتاخد الـ Page والـ PageSize من الـ Query String أوتوماتيك
            var result = await _mediator.Send(query, cancellationToken);
            return result.ToActionResult(this);
        }

        #endregion

        #region Join
        
        [HttpPost("join")]
        public async Task<IActionResult> Join([FromBody] JoinMeetingCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        } 

        #endregion
    }
}
