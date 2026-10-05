namespace EduNova.Domain.Common;

/// <summary>
/// Generic base for all domain entities: id + audit timestamps.
/// New entities should prefer <see cref="GuidKeyEntity"/> (auto Guid v7 id)
/// or inherit <see cref="BaseEntity{TId}"/> directly for other key types.
/// </summary>
public abstract class BaseEntity<TId>
{
    public TId Id { get; set; } = default!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }
}

/// <summary>
/// Int-keyed base kept for backward compatibility so existing
/// references to BaseEntity keep compiling unchanged.
/// </summary>
public abstract class BaseEntity : BaseEntity<int>
{
}

/// <summary>
/// Base for Guid-keyed entities. Generates a Guid v7 on construction —
/// time-ordered values keep SQL Server clustered indexes healthy, and the
/// format matches the Lecture module's ids. The Id stays settable so seed
/// data can pin deterministic values.
/// </summary>
public abstract class GuidKeyEntity : BaseEntity<Guid>
{
    public GuidKeyEntity() => Id = Guid.CreateVersion7();
}
