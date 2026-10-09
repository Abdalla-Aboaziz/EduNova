using EduNova.Application.Features.Events.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Events.Validations;

public class SetReminderCommandValidator : AbstractValidator<SetReminderCommand>
{
    public SetReminderCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.EventId)
            .NotEmpty().WithMessage("The event id is required.");

        RuleFor(x => x.RemindAt)
            .NotEmpty().WithMessage("The reminder time is required.")
            .GreaterThan(DateTime.UtcNow).WithMessage("The reminder time must be in the future.");

        RuleFor(x => x.Message)
            .MaximumLength(1000).WithMessage("The reminder message must not exceed 1000 characters.");
    }
}
