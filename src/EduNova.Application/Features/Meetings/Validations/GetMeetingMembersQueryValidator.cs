using EduNova.Application.Features.Meetings.Queries;
using FluentValidation;

namespace EduNova.Application.Features.Meetings.Validations;

public class GetMeetingMembersQueryValidator : AbstractValidator<GetMeetingMembersQuery>
{
    public GetMeetingMembersQueryValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.MeetingId)
            .NotEmpty().WithMessage("Meeting id is required.");
    }
}