namespace EduNova.Application.Contracts.Services;

public interface IMeetingLiveNotifier
{
    Task MemberJoinedAsync(string room, string userId, CancellationToken cancellationToken = default);
    Task MemberLeftAsync(string room, string userId, CancellationToken cancellationToken = default);
    Task MeetingEndedAsync(string room, Guid meetingId, CancellationToken cancellationToken = default);
}