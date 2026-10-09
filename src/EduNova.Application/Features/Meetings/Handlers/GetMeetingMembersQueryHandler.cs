using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Meetings.Queries;
using EduNova.Application.Features.Meetings.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers;

public sealed class GetMeetingMembersQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetMeetingMembersQuery, Result<List<MeetingMemberResponse>>>
{
    public async Task<Result<List<MeetingMemberResponse>>> Handle(GetMeetingMembersQuery request, CancellationToken cancellationToken)
    {
        var exists = await context.Meetings
            .AsNoTracking()
            .AnyAsync(m => m.Id == request.MeetingId, cancellationToken);

        if (!exists)
        {
            return Result.Failure<List<MeetingMemberResponse>>(Error.NotFound("Meeting", request.MeetingId));
        }

        var members = await context.MeetingParticipants
            .AsNoTracking()
            .Where(mp => mp.MeetingId == request.MeetingId)
            .OrderBy(mp => mp.JoinedAt)
            .Select(mp => new MeetingMemberResponse
            {
                UserId = mp.UserId,
                JoinedAt = mp.JoinedAt,
                LeftAt = mp.LeftAt,
                IsPresent = mp.LeftAt == null
            })
            .ToListAsync(cancellationToken);

        return Result.Success(members);
    }
}