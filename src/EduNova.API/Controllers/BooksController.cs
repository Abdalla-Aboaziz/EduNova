using EduNova.Application.Features.Book.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BooksController : ControllerBase
{
    private readonly IMediator _mediator;

    public BooksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetBooksQuery query, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(query, cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(
            statusCode: (int)result.Error.Type,
            detail: result.Error.Message,
            title: result.Error.Code);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetBookByIdQuery(id), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(
            statusCode: (int)result.Error.Type,
            detail: result.Error.Message,
            title: result.Error.Code);
    }

    [HttpGet("{id}/stream")]
    public async Task<IActionResult> Stream([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var (stream, contentType, fileName) = await _mediator.Send(new GetBookContentQuery(id), cancellationToken);
        return stream is null ? NotFound() : File(stream, contentType, fileName, true);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> Download([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var (fileContent, contentType, fileName) = await _mediator.Send(new DownloadBookContentQuery(id), cancellationToken);
        return fileContent.Length == 0 ? NotFound() : File(fileContent, contentType, fileName);
    }
}