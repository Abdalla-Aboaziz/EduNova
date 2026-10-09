using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Meetings.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers;

public sealed class LeaveMeetingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IMeetingLiveNotifier liveNotifier)
    : IRequestHandler<LeaveMeetingCommand, Result>
{
    public async Task<Result> Handle(LeaveMeetingCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        if (string.IsNullOrEmpty(userId))
        {
            return Result.Failure(Error.Forbidden("Auth.Unauthorized", "User is not authenticated."));
        }

        var meeting = await context.Meetings
            .FirstOrDefaultAsync(m => m.Id == request.MeetingId, cancellationToken);

        if (meeting is null)
        {
            return Result.Failure(Error.NotFound("Meeting", request.MeetingId));
        }

        var participation = await context.MeetingParticipants
            .FirstOrDefaultAsync(mp => mp.MeetingId == request.MeetingId && mp.UserId == userId, cancellationToken);

        if (participation is null)
        {
            return Result.Failure(Error.NotFound("Meeting.NotParticipant", "You have not joined this meeting."));
        }

        if (participation.LeftAt is null)
        {
            participation.LeftAt = DateTime.UtcNow;
            await context.SaveChangesAsync(cancellationToken);

            await liveNotifier.MemberLeftAsync(
                meeting.RoomId ?? meeting.Id.ToString("N"),
                userId,
                cancellationToken);
        }

        return Result.Success();
    }
}