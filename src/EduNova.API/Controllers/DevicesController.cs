using EduNova.API.Common;
using EduNova.Application.Features.Devices.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DevicesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DevicesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] RegisterDeviceTokenCommand command,
            CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return result.ToActionResult(this);
        }
    }
}
