using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Queries;
using EduNova.Application.Features.Meetings.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers
{
    public sealed class GetMeetingsQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetMeetingsQuery, Result<PagedResult<MeetingResponse>>>
    {
        public async Task<Result<PagedResult<MeetingResponse>>> Handle(GetMeetingsQuery request, CancellationToken cancellationToken)
        {
            var query = context.Meetings
                .AsNoTracking()
                .OrderByDescending(m => m.StartTime)
                .Select(m => new MeetingResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    Description = m.Description,
                    StartTime = m.StartTime,
                    IsVideoMeeting = m.IsVideoMeeting,
                    JoinCode = m.JoinCode,
                    IsActive = m.IsActive
                });

            var pagedMeetings = await query.ToPagedResultAsync(request, cancellationToken);

            return Result.Success(pagedMeetings);
        }
    }
}
