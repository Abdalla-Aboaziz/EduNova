using MediatR;
using EduNova.Application.Contracts.Services;

namespace EduNova.Application.Features.Authentication.Commands;

public class ResetPasswordByPhoneCommandHandler : IRequestHandler<ResetPasswordByPhoneCommand, Unit>
{
    private readonly IAuthService _authService;

    public ResetPasswordByPhoneCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(ResetPasswordByPhoneCommand request, CancellationToken cancellationToken)
    {
        await _authService.ResetPasswordByPhoneAsync(request.PhoneNumber, request.NewPassword);
        return Unit.Value;
    }
}
