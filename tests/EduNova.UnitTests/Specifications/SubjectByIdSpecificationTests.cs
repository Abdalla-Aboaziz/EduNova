using EduNova.Infrastructure.Data;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// The details-screen spec fetches exactly one subject by id.
/// </summary>
public class SubjectByIdSpecificationTests
{
    [Fact]
    public async Task Selects_only_the_subject_with_the_requested_id()
    {
        var target = Guid.NewGuid();
        await using var db = TestDb.Create();
        db.Subjects.AddRange(
            new Subject { Id = target, Code = "ENG101", Name = "English", CreditHours = 2, YearId = Guid.NewGuid(), SemesterId = Guid.NewGuid() },
            new Subject { Code = "MTH101", Name = "Math", CreditHours = 4, YearId = Guid.NewGuid(), SemesterId = Guid.NewGuid() });
        await db.SaveChangesAsync();

        var spec = new SubjectByIdSpecification(target);

        var names = await spec.ApplyTo(db.Subjects.AsNoTracking())
            .Select(s => s.Name)
            .ToListAsync();
        var subject = Assert.Single(names);
        Assert.Equal("English", subject);
    }

    [Fact]
    public async Task Unknown_id_yields_no_rows()
    {
        await using var db = TestDb.Create();
        db.Subjects.Add(new Subject { Code = "ENG101", Name = "English", CreditHours = 2, YearId = Guid.NewGuid(), SemesterId = Guid.NewGuid() });
        await db.SaveChangesAsync();

        var spec = new SubjectByIdSpecification(Guid.NewGuid());

        var rows = await spec.ApplyTo(db.Subjects.AsNoTracking()).ToListAsync();
        Assert.Empty(rows);
    }
}
