using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Queries;
using EduNova.Application.Features.Meetings.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers
{
    public sealed class GetMeetingByIdQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetMeetingByIdQuery, Result<MeetingResponse>>
    {
        public async Task<Result<MeetingResponse>> Handle(GetMeetingByIdQuery request, CancellationToken cancellationToken)
        {
            var meeting = await context.Meetings
                .AsNoTracking()
                .Where(m => m.Id == request.Id)
                .Select(m => new MeetingResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    StartTime = m.StartTime,
                    IsVideoMeeting = m.IsVideoMeeting,
                    JoinCode = m.JoinCode,
                    IsActive = m.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (meeting is null)
            {
                return Result.Failure<MeetingResponse>(Error.NotFound("Meeting", request.Id));
            }

            return Result.Success(meeting);
        }
    }
}
