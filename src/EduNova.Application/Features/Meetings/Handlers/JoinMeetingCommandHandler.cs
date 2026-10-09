using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Contracts.Services;
using EduNova.Application.Features.Meetings.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Meetings.Handlers
{
    public sealed class JoinMeetingCommandHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUserService,
        IMeetingLiveNotifier liveNotifier)
        : IRequestHandler<JoinMeetingCommand, Result>
    {
        public async Task<Result> Handle(JoinMeetingCommand request, CancellationToken cancellationToken)
        {
            // 1. Get the current user's ID
            var userId = currentUserService.UserId;
            if (string.IsNullOrEmpty(userId))
            {
                return Result.Failure(Error.Forbidden("Auth.Unauthorized", "User is not authenticated."));
            }

            // 2. We look for the meeting by the code and make sure it's working

            var meeting = await context.Meetings
                .FirstOrDefaultAsync(m => m.JoinCode == request.JoinCode && m.IsActive, cancellationToken);

            if (meeting is null)
            {
                return Result.Failure(Error.NotFound("Meeting.NotFound", "Invalid or inactive join code."));
            }
            // 3. We make sure the student hasn’t joined before so we can prevent duplication

            var alreadyJoined = await context.MeetingParticipants
                .AnyAsync(mp => mp.MeetingId == meeting.Id && mp.UserId == userId, cancellationToken);

            if (alreadyJoined)
            {
                return Result.Failure(Error.Conflict("Meeting.AlreadyJoined", "You have already joined this meeting."));
            }
            // 4. We register the student in the meeting

            var participant = new MeetingParticipant
            {
                Id = Guid.NewGuid(),
                MeetingId = meeting.Id,
                UserId = userId,
                JoinedAt = DateTime.UtcNow
            };

            context.MeetingParticipants.Add(participant);
            await context.SaveChangesAsync(cancellationToken);

            await liveNotifier.MemberJoinedAsync(
                meeting.RoomId ?? meeting.Id.ToString("N"),
                userId,
                cancellationToken);

            return Result.Success();
        }
    }
}
