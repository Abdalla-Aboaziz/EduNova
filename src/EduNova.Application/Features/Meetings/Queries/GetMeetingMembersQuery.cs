using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Responses;
using MediatR;

namespace EduNova.Application.Features.Meetings.Queries;

public class GetMeetingMembersQuery(Guid meetingId) : IRequest<Result<List<MeetingMemberResponse>>>
{
    public Guid MeetingId { get; set; } = meetingId;
}