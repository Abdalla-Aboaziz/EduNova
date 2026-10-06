using EduNova.API.Common;
using EduNova.Application.Features.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InstructorsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public InstructorsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Paged instructor list with name search (Instructors screen).</summary>
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] GetInstructorsQuery query, CancellationToken cancellationToken)
            => (await _mediator.Send(query, cancellationToken)).ToActionResult(this);
    }
}
