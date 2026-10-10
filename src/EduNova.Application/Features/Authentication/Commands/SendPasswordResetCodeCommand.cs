using MediatR;

namespace EduNova.Application.Features.Authentication.Commands;

public record SendPasswordResetCodeCommand(string PhoneNumber) : IRequest<Unit>;
