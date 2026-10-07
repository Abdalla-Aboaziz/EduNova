using EduNova.Infrastructure.Data;
using EduNova.Infrastructure.Data.Seed;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace EduNova.UnitTests.Grades;

/// <summary>
/// Seeder checks: full catalog + 14 demo grades on a fresh database, and
/// running the seeder again changes nothing (idempotent per block — so
/// databases seeded before the grades step still pick the grades up).
/// </summary>
public class DbSeederTests
{
    [Fact]
    public async Task Seeds_catalog_and_grades_together_on_a_fresh_database()
    {
        await using var db = TestDb.Create();

        await DbSeeder.SeedAsync(db, NullLogger.Instance);

        Assert.Equal(4, await db.Years.CountAsync());
        Assert.Equal(8, await db.Semesters.CountAsync());
        Assert.Equal(16, await db.Subjects.CountAsync());
        Assert.Equal(8, await db.Instructors.CountAsync());
        Assert.Equal(18, await db.Offers.CountAsync());
        Assert.Equal(14, await db.Grades.CountAsync());

        var grades = await db.Grades.ToListAsync();
        Assert.All(grades, g => Assert.Equal("demo-student-1", g.StudentId));
        Assert.Equal(30m, grades[0].MaxScore);
        Assert.NotNull(grades[0].ExamAt);
    }

    [Fact]
    public async Task Running_the_seeder_again_changes_nothing()
    {
        await using var db = TestDb.Create();
        await DbSeeder.SeedAsync(db, NullLogger.Instance);

        await DbSeeder.SeedAsync(db, NullLogger.Instance);

        Assert.Equal(4, await db.Years.CountAsync());
        Assert.Equal(18, await db.Offers.CountAsync());
        Assert.Equal(14, await db.Grades.CountAsync());
    }

    [Fact]
    public async Task Grades_seed_on_a_database_that_already_has_catalog_data()
    {
        // Simulates databases created before T4.5: catalog present, no grades.
        await using var db = TestDb.Create();
        await DbSeeder.SeedAsync(db, NullLogger.Instance);
        db.Grades.RemoveRange(db.Grades);
        await db.SaveChangesAsync();

        await DbSeeder.SeedAsync(db, NullLogger.Instance);

        Assert.Equal(14, await db.Grades.CountAsync());
    }
}
