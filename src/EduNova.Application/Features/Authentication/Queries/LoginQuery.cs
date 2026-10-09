using EduNova.Application.Features.Authentication.DTOs;
using MediatR;

namespace EduNova.Application.Features.Authentication.Queries;

public class LoginQuery : IRequest<AuthResponseDto>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}