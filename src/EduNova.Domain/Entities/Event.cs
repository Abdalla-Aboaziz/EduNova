using EduNova.Domain.Common;

namespace EduNova.Domain.Entities;

public sealed class Event : GuidKeyEntity
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public EventType Type { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public string? Location { get; set; }
    public Guid? SubjectId { get; set; }

    public Subject? Subject { get; set; }
    public ICollection<Reminder> Reminders { get; set; } = [];
}
