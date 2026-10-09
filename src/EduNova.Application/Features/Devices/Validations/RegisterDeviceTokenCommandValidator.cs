using EduNova.Application.Features.Devices.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Devices.Validations;

public class RegisterDeviceTokenCommandValidator : AbstractValidator<RegisterDeviceTokenCommand>
{
    public RegisterDeviceTokenCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.Token)
            .NotEmpty().WithMessage("The device token is required.")
            .MaximumLength(500).WithMessage("The device token must not exceed 500 characters.");

        RuleFor(x => x.Platform)
            .NotEmpty().WithMessage("The platform is required.")
            .MaximumLength(50).WithMessage("The platform must not exceed 50 characters.");
    }
}
