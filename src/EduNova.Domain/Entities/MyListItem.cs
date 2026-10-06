namespace EduNova.Domain.Entities
{
    public sealed class MyListItem :  BaseEntity<Guid>
    {
        
        public string UserId { get; set; } = string.Empty;
        public Guid LectureId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public Lecture Lecture { get; set; } = null!;
    }
}
