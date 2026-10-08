namespace EduNova.Domain.Entities;

public class Note : BaseEntity<int>
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public NoteColor Color { get; set; }

    public string UserId { get; set; } = string.Empty;
    // public ApplicationUser User { get; set; } = null!;
}
