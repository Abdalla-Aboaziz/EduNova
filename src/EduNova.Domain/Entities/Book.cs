namespace EduNova.Domain.Entities;

public class Book : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? SubjectId { get; set; }
    public Guid FileId { get; set; }

    public Subject? Subject { get; set; }
    public UploadedFiles? File { get; set; }
}