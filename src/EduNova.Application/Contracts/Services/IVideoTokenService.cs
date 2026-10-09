using EduNova.Application.Features.Meetings.Responses;

namespace EduNova.Application.Contracts.Services;

public interface IVideoTokenService
{
    Task<MeetingTokenResponse> GenerateAsync(Guid meetingId, string room, string userId, CancellationToken cancellationToken = default);
}