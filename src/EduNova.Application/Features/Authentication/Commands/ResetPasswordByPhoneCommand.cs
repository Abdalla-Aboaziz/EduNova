using MediatR;

namespace EduNova.Application.Features.Authentication.Commands;

public record ResetPasswordByPhoneCommand(string PhoneNumber, string NewPassword) : IRequest<Unit>;
