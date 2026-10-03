namespace EduNova.Domain.Entities
{
    public sealed class Lecture
    {
        public Guid Id { get; set; } = Guid.CreateVersion7();
        public Guid SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public int Order { get; set; }
        public int DurationInSeconds { get; set; }
        public Guid VideoFileId { get; set; }
        public Guid? ThumbnailFileId { get; set; }
        public string UploadedById { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public UploadedFiles VideoFile { get; set; } = null!;
        public UploadedFiles? ThumbnailFile { get; set; }
        public ICollection<MyListItem> MyListItems { get; set; } = [];
        public ICollection<DownloadedLecture> DownloadedLectures { get; set; } = [];
    }
}
