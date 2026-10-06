namespace EduNova.Domain.Entities
{
    public sealed class DownloadedLecture : BaseEntity<Guid>
    {
       
        public string UserId { get; set; }
        public Guid LectureId { get; set; }
        public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;

        public Lecture Lecture { get; set; } = null!;
    }
}
