using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Features.Lecture.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LecturesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetBySubject([FromRoute] Guid subjectId, CancellationToken cancellationToken)
       => Ok(await _mediator.Send(new GetSubjectLecturesQuery(subjectId), cancellationToken));

        [HttpPost]
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(524_288_000)]
        [RequestFormLimits(MultipartBodyLengthLimit = 524_288_000)]
        public async Task<IActionResult> Upload([FromForm] UploadLectureCommand command, CancellationToken cancellationToken)
        {
            var id = await _mediator.Send(command, cancellationToken);
            return CreatedAtAction(nameof(Stream), new { id }, null);
        }

        [HttpGet("{id}/stream")]
        public async Task<IActionResult> Stream([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var (stream, contentType, fileName) = await _mediator.Send(new GetLectureStreamQuery(id), cancellationToken);
            return stream is null ? NotFound() : File(stream, contentType, fileName, true);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
            => await _mediator.Send(new DeleteLectureCommand(id), cancellationToken) ? NoContent() : NotFound();
    }
}

