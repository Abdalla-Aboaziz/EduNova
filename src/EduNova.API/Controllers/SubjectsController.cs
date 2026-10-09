using EduNova.API.Common;
using EduNova.Application.Features.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SubjectsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SubjectsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>Paged subject list with search and year/semester filters (Filter screen).</summary>
        [HttpGet]
        public async Task<IActionResult> List([FromQuery] GetSubjectsQuery query, CancellationToken cancellationToken)
            => (await _mediator.Send(query, cancellationToken)).ToActionResult(this);

        /// <summary>Subject details for the My Subject screen (404 when unknown).</summary>
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
            => (await _mediator.Send(new GetSubjectByIdQuery(id), cancellationToken)).ToActionResult(this);
    }
}
