using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
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
    public DbSet<Lecture> Lectures => Set<Lecture>();
    public DbSet<MyListItem> MyListItems => Set<MyListItem>();
    public DbSet<DownloadedLecture> DownloadedLectures => Set<DownloadedLecture>();

    public DbSet<UploadedFiles> Files => Set<UploadedFiles>();

    public DbSet<Year> Years => Set<Year>();
    public DbSet<Semester> Semesters => Set<Semester>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<Instructor> Instructors => Set<Instructor>();
    public DbSet<Offer> Offers => Set<Offer>();
    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingParticipant> MeetingParticipants => Set<MeetingParticipant>();
}
