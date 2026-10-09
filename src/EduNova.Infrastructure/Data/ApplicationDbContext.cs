using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid> ,IApplicationDbContext
{

    /// <summary>
    /// EF Core DbContext implementation for the EduNova application.
    /// Implements IApplicationDbContext so the Application layer can use it
    /// via dependency injection without referencing Infrastructure directly.
    /// </summary>

   
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }
    // Example: public DbSet<Course> Courses => Set<Course>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Apply all entity configurations from this assembly
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Lecture> Lectures => Set<Lecture>();
        public DbSet<Note> Notes => Set<Note>();
        public DbSet<Book> Books => Set<Book>();
        public DbSet<MyListItem> MyListItems => Set<MyListItem>();
        public DbSet<DownloadedLecture> DownloadedLectures => Set<DownloadedLecture>();

        public DbSet<UploadedFiles> Files => Set<UploadedFiles>();

        public DbSet<Year> Years => Set<Year>();
        public DbSet<Semester> Semesters => Set<Semester>();
        public DbSet<Subject> Subjects => Set<Subject>();
        public DbSet<Instructor> Instructors => Set<Instructor>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Grade> Grades => Set<Grade>();

    public DbSet<Meeting> Meetings => Set<Meeting>();
    public DbSet<MeetingParticipant> MeetingParticipants => Set<MeetingParticipant>();

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Reminder> Reminders => Set<Reminder>();
    public DbSet<AttendanceRecord> AttendanceRecords => Set<AttendanceRecord>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<DeviceToken> DeviceTokens => Set<DeviceToken>();
}
