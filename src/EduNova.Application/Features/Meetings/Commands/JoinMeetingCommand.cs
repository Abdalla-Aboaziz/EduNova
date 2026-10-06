using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Meetings.Commands
{
    public class JoinMeetingCommand : IRequest<Result>
    {
        public string JoinCode { get; set; } = string.Empty;
    }
}
