using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Responses;
using MediatR;

namespace EduNova.Application.Features.Meetings.Commands
{
    public class CreateMeetingCommand : IRequest<Result<CreateMeetingResponse>>
    {
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsVideoMeeting { get; set; }
    }
}
