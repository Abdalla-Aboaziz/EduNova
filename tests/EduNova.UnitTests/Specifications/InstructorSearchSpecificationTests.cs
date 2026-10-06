using EduNova.Infrastructure.Data;
using Xunit;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// Filters/search/order checks for the Instructors screen spec.
/// </summary>
public class InstructorSearchSpecificationTests
{
    private static async Task<ApplicationDbContext> CreateSeededDbAsync()
    {
        var db = TestDb.Create();
        db.Instructors.AddRange(
            new Instructor { FullName = "Dr. Ahmed Hassan", AcademicTitle = "Professor" },
            new Instructor { FullName = "Dr. Ranny Ali", AcademicTitle = "Lecturer" },
            new Instructor { FullName = "Dr. Sara Mahmoud", AcademicTitle = "Professor" });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task Returns_all_instructors_ordered_by_name_when_no_search()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorSearchSpecification(null);

        var names = await spec.ApplyTo(db.Instructors.AsNoTracking())
            .Select(i => i.FullName)
            .ToListAsync();
        Assert.Equal(["Dr. Ahmed Hassan", "Dr. Ranny Ali", "Dr. Sara Mahmoud"], names);
    }

    [Fact]
    public async Task Searches_by_full_name()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorSearchSpecification("Sara");

        var names = await spec.ApplyTo(db.Instructors.AsNoTracking())
            .Select(i => i.FullName)
            .ToListAsync();
        Assert.Equal(["Dr. Sara Mahmoud"], names);
    }

    [Fact]
    public async Task Search_with_no_matches_returns_empty_result()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorSearchSpecification("nobody");

        var names = await spec.ApplyTo(db.Instructors.AsNoTracking())
            .Select(i => i.FullName)
            .ToListAsync();
        Assert.Empty(names);
    }
}
