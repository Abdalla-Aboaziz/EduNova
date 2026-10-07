using EduNova.Infrastructure.Data;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// The Dr. Details spec fetches exactly one instructor by id.
/// </summary>
public class InstructorByIdSpecificationTests
{
    [Fact]
    public async Task Selects_only_the_instructor_with_the_requested_id()
    {
        var target = Guid.NewGuid();
        await using var db = TestDb.Create();
        db.Instructors.AddRange(
            new Instructor { Id = target, FullName = "Dr. Reham Ahmed", AcademicTitle = "Professor" },
            new Instructor { FullName = "Dr. Ahmed Hassan", AcademicTitle = "Professor" });
        await db.SaveChangesAsync();

        var spec = new InstructorByIdSpecification(target);

        var names = await spec.ApplyTo(db.Instructors.AsNoTracking())
            .Select(i => i.FullName)
            .ToListAsync();
        var instructor = Assert.Single(names);
        Assert.Equal("Dr. Reham Ahmed", instructor);
    }

    [Fact]
    public async Task Unknown_id_yields_no_rows()
    {
        await using var db = TestDb.Create();
        db.Instructors.Add(new Instructor { FullName = "Dr. Reham Ahmed", AcademicTitle = "Professor" });
        await db.SaveChangesAsync();

        var spec = new InstructorByIdSpecification(Guid.NewGuid());

        var rows = await spec.ApplyTo(db.Instructors.AsNoTracking()).ToListAsync();
        Assert.Empty(rows);
    }
}
