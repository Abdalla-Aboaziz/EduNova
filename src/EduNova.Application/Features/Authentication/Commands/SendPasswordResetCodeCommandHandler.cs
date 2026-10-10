using MediatR;
using EduNova.Application.Contracts.Services;

namespace EduNova.Application.Features.Authentication.Commands;

public class SendPasswordResetCodeCommandHandler : IRequestHandler<SendPasswordResetCodeCommand, Unit>
{
    private readonly IAuthService _authService;

    public SendPasswordResetCodeCommandHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<Unit> Handle(SendPasswordResetCodeCommand request, CancellationToken cancellationToken)
    {
        await _authService.SendPasswordResetCodeAsync(request.PhoneNumber);
        return Unit.Value;
    }
}
