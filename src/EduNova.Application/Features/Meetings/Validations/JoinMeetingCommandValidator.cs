using EduNova.Application.Features.Meetings.Commands;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations
{
    public class JoinMeetingCommandValidator : AbstractValidator<JoinMeetingCommand>
    {
        public JoinMeetingCommandValidator()
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.JoinCode)
                .NotEmpty().WithMessage("Join code is required.")
                .Length(6).WithMessage("Join code must be exactly 6 characters.");
        }
    }
}
