using EduNova.Application.Features.Profile.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Profile.Validations;

public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
{
    public UpdateProfileCommandValidator()
    {
        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(100);
    }
}