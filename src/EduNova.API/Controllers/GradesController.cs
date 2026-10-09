using EduNova.API.Common;
using EduNova.Application.Features.Grades.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/grades")]
    [ApiController]
    [Authorize]
    public class GradesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public GradesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// The current student's grade sheet, optionally narrowed by year and
        /// semester. Always scoped to the authenticated (or demo) student.
        /// </summary>
        [HttpGet("sheet")]
        public async Task<IActionResult> Sheet(
            [FromQuery] Guid? yearId,
            [FromQuery] Guid? semesterId,
            CancellationToken cancellationToken)
            => (await _mediator.Send(new GetGradeSheetQuery(yearId, semesterId), cancellationToken))
                .ToActionResult(this);

        /// <summary>Chart data (GPA trend + letter distribution) for the current student.</summary>
        [HttpGet("chart")]
        public async Task<IActionResult> Chart(CancellationToken cancellationToken)
            => (await _mediator.Send(new GetGradeChartQuery(), cancellationToken)).ToActionResult(this);
    }
}
