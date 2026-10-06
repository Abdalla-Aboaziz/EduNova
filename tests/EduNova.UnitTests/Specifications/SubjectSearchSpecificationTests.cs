using EduNova.Infrastructure.Data;
using Xunit;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// Requirement checks 1-3 for the Filter screen spec: filtering by year and
/// semester, searching by name/code, and stable ordering by name.
/// </summary>
public class SubjectSearchSpecificationTests
{
    private static readonly Guid Year1 = Guid.NewGuid();
    private static readonly Guid Year2 = Guid.NewGuid();
    private static readonly Guid Semester1 = Guid.NewGuid();
    private static readonly Guid Semester2 = Guid.NewGuid();

    private static async Task<ApplicationDbContext> CreateSeededDbAsync()
    {
        var db = TestDb.Create();
        db.Subjects.AddRange(
            new Subject { Code = "ENG101", Name = "English", CreditHours = 2, YearId = Year1, SemesterId = Semester1 },
            new Subject { Code = "MTH101", Name = "Math", CreditHours = 4, YearId = Year1, SemesterId = Semester1 },
            new Subject { Code = "PHY101", Name = "Physics", CreditHours = 3, YearId = Year1, SemesterId = Semester2 },
            new Subject { Code = "DBI201", Name = "Databases", CreditHours = 3, YearId = Year2, SemesterId = Semester2 });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task Filters_by_year()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new SubjectSearchSpecification(search: null, yearId: Year1, semesterId: null);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        Assert.Equal(["English", "Math", "Physics"], names);
    }

    [Fact]
    public async Task Filters_by_semester()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new SubjectSearchSpecification(search: null, yearId: null, semesterId: Semester1);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        Assert.Equal(["English", "Math"], names);
    }

    [Fact]
    public async Task Searches_by_name_or_code()
    {
        await using var db = await CreateSeededDbAsync();

        var byName = new SubjectSearchSpecification("Data", null, null);
        var byCode = new SubjectSearchSpecification("ENG", null, null);

        var nameMatches = await byName.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        var codeMatches = await byCode.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();

        Assert.Equal(["Databases"], nameMatches);
        Assert.Equal(["English"], codeMatches);
    }

    [Fact]
    public async Task Combines_year_semester_and_search_filters()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new SubjectSearchSpecification("Phy", Year1, Semester2);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        Assert.Equal(["Physics"], names);
    }

    [Fact]
    public async Task Orders_by_name_when_no_filters_are_set()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new SubjectSearchSpecification(null, null, null);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        Assert.Equal(["Databases", "English", "Math", "Physics"], names);
    }

    [Fact]
    public async Task Search_that_matches_nothing_returns_empty_query_result()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new SubjectSearchSpecification("does-not-exist", null, null);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        Assert.Empty(names);
    }
}
