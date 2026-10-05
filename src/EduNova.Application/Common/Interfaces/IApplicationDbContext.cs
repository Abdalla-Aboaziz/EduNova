using EduNova.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Interfaces;

/// <summary>
/// Abstraction over the EF Core DbContext for the Application layer.
/// Infrastructure implements this; Application depends on it (Dependency Inversion).
/// Add DbSet properties here as you create entities.
/// </summary>
public interface IApplicationDbContext
{
    DbSet<Lecture> Lectures { get; }
    DbSet<MyListItem> MyListItems { get; }
    DbSet<DownloadedLecture> DownloadedLectures { get; }
    DbSet<UploadedFiles> Files { get; }

    DbSet<Year> Years { get; }
    DbSet<Semester> Semesters { get; }
    DbSet<Subject> Subjects { get; }
    DbSet<Instructor> Instructors { get; }
    DbSet<Offer> Offers { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
