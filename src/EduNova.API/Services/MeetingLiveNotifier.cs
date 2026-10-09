using EduNova.API.Hubs;
using EduNova.Application.Contracts.Services;
using Microsoft.AspNetCore.SignalR;

namespace EduNova.API.Services;

public sealed class MeetingLiveNotifier(IHubContext<MeetingHub> hubContext) : IMeetingLiveNotifier
{
    public Task MemberJoinedAsync(string room, string userId, CancellationToken cancellationToken = default)
        => hubContext.Clients.Group(room).SendAsync("MemberJoined", new { room, userId }, cancellationToken);

    public Task MemberLeftAsync(string room, string userId, CancellationToken cancellationToken = default)
        => hubContext.Clients.Group(room).SendAsync("MemberLeft", new { room, userId }, cancellationToken);

    public Task MeetingEndedAsync(string room, Guid meetingId, CancellationToken cancellationToken = default)
        => hubContext.Clients.Group(room).SendAsync("MeetingEnded", new { room, meetingId }, cancellationToken);
}