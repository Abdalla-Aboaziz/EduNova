namespace EduNova.Application.Features.Meetings.Responses
{
    public class CreateMeetingResponse
    {
        public Guid MeetingId { get; set; }
        public string JoinCode { get; set; } = string.Empty;
    }
}
