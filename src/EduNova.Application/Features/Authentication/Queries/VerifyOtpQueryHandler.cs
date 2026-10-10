using MediatR;
using EduNova.Application.Contracts.Services;

namespace EduNova.Application.Features.Authentication.Queries;

public class VerifyOtpQueryHandler : IRequestHandler<VerifyOtpQuery, bool>
{
    private readonly IAuthService _authService;

    public VerifyOtpQueryHandler(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task<bool> Handle(VerifyOtpQuery request, CancellationToken cancellationToken)
    {
        return await _authService.VerifyPasswordResetCodeAsync(request.PhoneNumber, request.Code);
    }
}
