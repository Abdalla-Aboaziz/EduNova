using EduNova.Application.Features.Note.Command;
using EduNova.Application.Features.Note.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NoteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NoteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllNotesQuery());
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetNoteByIdQuery { Id = id });
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddNoteCommand command)
        {
            var result = await _mediator.Send(command);
            return Created(string.Empty, result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteCommand command)
        {
            command.Id = id;
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteNoteCommand { Id = id });
            return Ok(result);
        }
    }
}
