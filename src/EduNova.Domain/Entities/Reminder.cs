using EduNova.Domain.Common;

namespace EduNova.Domain.Entities;

public sealed class Reminder : GuidKeyEntity
{
    public string UserId { get; set; } = string.Empty;
    public Guid EventId { get; set; }
    public string? Message { get; set; }
    public DateTime RemindAt { get; set; }
    public bool IsSent { get; set; }
    public DateTime? SentAt { get; set; }

    public Event Event { get; set; } = null!;
}
