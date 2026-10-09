using EduNova.Application.Features.Meetings.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations;

public class LeaveMeetingCommandValidator : AbstractValidator<LeaveMeetingCommand>
{
    public LeaveMeetingCommandValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.MeetingId)
            .NotEmpty().WithMessage("Meeting id is required.");
    }
}