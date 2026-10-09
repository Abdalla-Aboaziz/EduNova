using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Meetings.Queries;
using EduNova.Application.Features.Meetings.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers;

public sealed class GetMeetingTokenQueryHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IVideoTokenService videoTokenService)
    : IRequestHandler<GetMeetingTokenQuery, Result<MeetingTokenResponse>>
{
    public async Task<Result<MeetingTokenResponse>> Handle(GetMeetingTokenQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure<MeetingTokenResponse>(Error.Forbidden("Auth.Unauthorized", "User is not authenticated."));
        }

        var meeting = await context.Meetings
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == request.MeetingId && m.IsActive, cancellationToken);

        if (meeting is null)
        {
            return Result.Failure<MeetingTokenResponse>(Error.NotFound("Meeting.NotFound", "Invalid or inactive meeting."));
        }

        var isParticipant = await context.MeetingParticipants
            .AsNoTracking()
            .AnyAsync(mp => mp.MeetingId == request.MeetingId && mp.UserId == userId && mp.LeftAt == null, cancellationToken);

        if (!isParticipant)
        {
            return Result.Failure<MeetingTokenResponse>(Error.Forbidden("Meeting.NotParticipant", "Join the meeting before requesting a video token."));
        }

        var token = await videoTokenService.GenerateAsync(
            meeting.Id,
            meeting.RoomId ?? meeting.Id.ToString("N"),
            userId,
            cancellationToken);

        return Result.Success(token);
    }
}