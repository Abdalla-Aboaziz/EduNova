using MediatR;
using EduNova.Application.Features.Authentication.Commands;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.DTOs;

namespace EduNova.Application.Features.Authentication.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public RegisterCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = new RegisterDto
        {
            DisplayName = request.DisplayName,
            Email = request.Email,
            Password = request.Password
        };

        return await _authService.RegisterAsync(dto);
    }
}
