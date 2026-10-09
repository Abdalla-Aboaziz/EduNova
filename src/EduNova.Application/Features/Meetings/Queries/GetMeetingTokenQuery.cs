using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Responses;
using MediatR;

namespace EduNova.Application.Features.Meetings.Queries;

public class GetMeetingTokenQuery(Guid meetingId) : IRequest<Result<MeetingTokenResponse>>
{
    public Guid MeetingId { get; set; } = meetingId;
}