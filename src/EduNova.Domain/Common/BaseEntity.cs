namespace EduNova.Domain.Common;

/// <summary>
/// Base entity class that all domain entities inherit from.
/// Provides common properties like Id, CreatedAt, and UpdatedAt.
/// </summary>
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
