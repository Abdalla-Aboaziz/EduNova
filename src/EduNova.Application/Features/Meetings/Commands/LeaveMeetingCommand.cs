using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Meetings.Commands;

public class LeaveMeetingCommand : IRequest<Result>
{
    public Guid MeetingId { get; set; }
}