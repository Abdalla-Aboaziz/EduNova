using EduNova.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Infrastructure.Data;

/// <summary>
/// EF Core DbContext implementation for the EduNova application.
/// Implements IApplicationDbContext so the Application layer can use it
/// via dependency injection without referencing Infrastructure directly.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Add your DbSet<TEntity> properties here as you create domain entities.
    // Example: public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Apply all entity configurations from this assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }


}
