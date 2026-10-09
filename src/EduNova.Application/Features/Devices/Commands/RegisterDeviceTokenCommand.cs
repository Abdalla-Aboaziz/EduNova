using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Devices.Commands;

public class RegisterDeviceTokenCommand : IRequest<Result>
{
    public string Token { get; set; } = string.Empty;
    public string Platform { get; set; } = string.Empty;
}
