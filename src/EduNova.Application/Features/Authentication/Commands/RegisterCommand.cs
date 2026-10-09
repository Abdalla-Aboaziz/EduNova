using MediatR;

namespace EduNova.Application.Features.Authentication.Commands;

public record RegisterCommand(string DisplayName, string Email, string Password) : IRequest<DTOs.AuthResponseDto>;
