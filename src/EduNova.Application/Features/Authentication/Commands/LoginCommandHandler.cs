using MediatR;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Authentication.DTOs;

namespace EduNova.Application.Features.Authentication.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthResponseDto>
{
    private readonly IAuthService _authService;

    public LoginCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dto = new LoginDto
        {
            Email = request.Email,
            Password = request.Password
        };

        return await _authService.LoginAsync(dto);
    }
}
