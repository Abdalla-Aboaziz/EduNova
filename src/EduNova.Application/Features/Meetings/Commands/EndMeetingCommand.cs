using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Meetings.Commands;

public class EndMeetingCommand : IRequest<Result>
{
    public Guid MeetingId { get; set; }
}