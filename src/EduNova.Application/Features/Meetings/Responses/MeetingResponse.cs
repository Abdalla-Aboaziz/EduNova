namespace EduNova.Application.Features.Meetings.Responses
{
    public class MeetingResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public bool IsVideoMeeting { get; set; }
        public string JoinCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}
