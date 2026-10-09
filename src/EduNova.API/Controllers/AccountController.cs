using MediatR;
using EduNova.Application.Features.Authentication.DTOs;
using EduNova.Application.Features.Authentication.Commands;
using EduNova.Application.Features.Authentication.Queries;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduNova.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var result = await _mediator.Send(new RegisterCommand(dto.DisplayName, dto.Email, dto.Password));
            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var result = await _mediator.Send(new LoginCommand(dto.Email, dto.Password));
            return Ok(result);
        }

        [HttpPost("send-code")]
        public async Task<IActionResult> SendPasswordResetCode([FromBody] SendOtpDto dto)
        {
            await _mediator.Send(new SendPasswordResetCodeCommand(dto.PhoneNumber));
            return Ok();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpDto dto)
        {
            var ok = await _mediator.Send(new VerifyOtpQuery(dto.PhoneNumber, dto.Code));
            return Ok(new { verified = ok });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordByPhoneDto dto)
        {
            await _mediator.Send(new ResetPasswordByPhoneCommand(dto.PhoneNumber, dto.NewPassword));
            return Ok();
        }
    }
}
