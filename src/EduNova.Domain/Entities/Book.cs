namespace EduNova.Domain.Entities;

public class Book : BaseEntity<Guid>
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? SubjectId { get; set; }         
    public bool IsActive { get; set; } = true;
    public ICollection<BookPage> Pages { get; set; } = new List<BookPage>();
}