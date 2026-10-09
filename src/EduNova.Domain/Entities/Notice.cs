using EduNova.Domain.Common;

namespace EduNova.Domain.Entities;

public sealed class Notice : GuidKeyEntity
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DateTime PublishedAt { get; set; } = DateTime.UtcNow;
    public bool IsActive { get; set; } = true;
}
