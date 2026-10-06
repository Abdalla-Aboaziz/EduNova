using EduNova.Application.Features.Meetings.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations
{
    public class CreateMeetingCommandValidator : AbstractValidator<CreateMeetingCommand>
    {
        public CreateMeetingCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("The meeting title is required.")
                .MaximumLength(200).WithMessage("The meeting title must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("The description must not exceed 1000 characters.");

            RuleFor(x => x.StartTime)
                .NotEmpty().WithMessage("The meeting start time is required.")
                .GreaterThan(DateTime.UtcNow).WithMessage("The meeting start time must be in the future.");
        }
    }
}
