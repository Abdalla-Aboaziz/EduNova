using EduNova.Application.Features.Profile.Commands;
using EduNova.Application.Features.Profile.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetCurrentUserQuery(), cancellationToken);
        return result.IsSuccess ? Ok(result.Value) : Problem(
            statusCode: (int)result.Error.Type,
            detail: result.Error.Message,
            title: result.Error.Code);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateProfileCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.IsSuccess ? Ok() : Problem(
            statusCode: (int)result.Error.Type,
            detail: result.Error.Message,
            title: result.Error.Code);
    }
}