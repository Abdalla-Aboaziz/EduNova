using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Profile.Commands;

public class UpdateProfileCommand : IRequest<Result>
{
    public string DisplayName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
}