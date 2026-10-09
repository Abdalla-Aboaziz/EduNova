namespace EduNova.Application.Features.Meetings.Responses;

public class MeetingMemberResponse
{
    public string UserId { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; }
    public DateTime? LeftAt { get; set; }
    public bool IsPresent { get; set; }
}