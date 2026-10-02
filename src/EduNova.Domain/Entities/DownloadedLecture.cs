namespace EduNova.Domain.Entities
{
    public sealed class DownloadedLecture
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid UserId { get; set; }
        public Guid LectureId { get; set; }
        public DateTime DownloadedAt { get; set; } = DateTime.UtcNow;
    }
}
