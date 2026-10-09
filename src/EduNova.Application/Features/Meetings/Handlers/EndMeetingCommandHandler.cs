using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Meetings.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers;

public sealed class EndMeetingCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUserService,
    IMeetingLiveNotifier liveNotifier)
    : IRequestHandler<EndMeetingCommand, Result>
{
    public async Task<Result> Handle(EndMeetingCommand request, CancellationToken cancellationToken)
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

        if (!meeting.IsActive)
        {
            return Result.Failure(Error.Conflict("Meeting.AlreadyEnded", "This meeting has already ended."));
        }

        meeting.IsActive = false;
        meeting.EndTime = DateTime.UtcNow;

        var activeParticipants = await context.MeetingParticipants
            .Where(mp => mp.MeetingId == request.MeetingId && mp.LeftAt == null)
            .ToListAsync(cancellationToken);

        foreach (var participant in activeParticipants)
        {
            participant.LeftAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        await liveNotifier.MeetingEndedAsync(
            meeting.RoomId ?? meeting.Id.ToString("N"),
            meeting.Id,
            cancellationToken);

        return Result.Success();
    }
}