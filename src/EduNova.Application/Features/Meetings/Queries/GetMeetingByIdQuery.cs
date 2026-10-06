using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Responses;
using MediatR;

namespace EduNova.Application.Features.Meetings.Queries
{
    public class GetMeetingByIdQuery(Guid id) : IRequest<Result<MeetingResponse>>
    {
        public Guid Id { get; set; } = id;
    }
}
