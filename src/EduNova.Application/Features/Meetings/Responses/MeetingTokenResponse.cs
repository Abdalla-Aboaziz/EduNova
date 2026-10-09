namespace EduNova.Application.Features.Meetings.Responses;

public class MeetingTokenResponse
{
    public string Token { get; set; } = string.Empty;
    public string Room { get; set; } = string.Empty;
    public string Provider { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}