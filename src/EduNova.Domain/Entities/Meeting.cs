using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    public class Meeting : GuidKeyEntity
    {
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string JoinCode { get; set; } = string.Empty;

        public DateTime StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public bool IsVideoMeeting { get; set; } = true;

        public bool IsActive { get; set; }

        public string? RoomId { get; set; }

        public ICollection<MeetingParticipant> Participants { get; set; } = new List<MeetingParticipant>();
    }
}