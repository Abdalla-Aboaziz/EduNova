using EduNova.Application.Features.Authentication.DTOs;
using MediatR;

namespace EduNova.Application.Features.Authentication.Commands;

public class RegisterCommand : IRequest<AuthResponseDto>
{
    public string DisplayName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}