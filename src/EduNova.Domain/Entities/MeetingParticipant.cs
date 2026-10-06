using EduNova.Domain.Common;

namespace EduNova.Domain.Entities
{
    public class MeetingParticipant : GuidKeyEntity
    {
        public Guid MeetingId { get; set; }
        public Meeting Meeting { get; set; } = null!;

        public string UserId { get; set; } = string.Empty;

        public DateTime JoinedAt { get; set; }
        public DateTime? LeftAt { get; set; }
    }
}