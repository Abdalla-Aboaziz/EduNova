using EduNova.Application.Features.Meetings.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations;

public class GetMeetingTokenQueryValidator : AbstractValidator<GetMeetingTokenQuery>
{
    public GetMeetingTokenQueryValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.MeetingId)
            .NotEmpty().WithMessage("Meeting id is required.");
    }
}