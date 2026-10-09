using MediatR;

namespace EduNova.Application.Features.Authentication.Commands;

public record LoginCommand(string Email, string Password) : IRequest<DTOs.AuthResponseDto>;
