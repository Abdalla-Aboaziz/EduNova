using EduNova.API.Common;
using EduNova.Application.Features.Catalog.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
    }
}
