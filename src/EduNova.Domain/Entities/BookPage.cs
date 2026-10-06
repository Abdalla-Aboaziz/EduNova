namespace EduNova.Domain.Entities;

public class BookPage : BaseEntity<int>
{
    public int BookId { get; set; }
    public Book Book { get; set; } = null!;

    public int PageNumber { get; set; }
    public string Content { get; set; } = string.Empty;
}