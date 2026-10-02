namespace EduNova.Domain.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext for the Application layer.
/// Infrastructure implements this; Application depends on it (Dependency Inversion).
/// Add DbSet properties here as you create entities.
/// </summary>
public interface IApplicationDbContext
{
    // Add your DbSet<TEntity> properties here as you create domain entities.
    // Example: DbSet<Course> Courses { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
