using EduNova.Infrastructure.Data;
using Xunit;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Domain.Entities;
using EduNova.UnitTests.Testing;
using Microsoft.EntityFrameworkCore;

namespace EduNova.UnitTests.Specifications;

/// <summary>
/// Dr. Details spec checks: offers of one instructor, narrowed by the
/// subject's year and the offer's semester, ordered by subject name,
/// with the subject navigation included.
/// </summary>
public class InstructorOffersSpecificationTests
{
    private static readonly Guid Year1 = Guid.NewGuid();
    private static readonly Guid Year2 = Guid.NewGuid();
    private static readonly Guid Semester1 = Guid.NewGuid();
    private static readonly Guid Semester2 = Guid.NewGuid();
    private static readonly Guid InstructorA = Guid.NewGuid();
    private static readonly Guid InstructorB = Guid.NewGuid();

    private static async Task<ApplicationDbContext> CreateSeededDbAsync()
    {
        var db = TestDb.Create();
        var english = new Subject { Code = "ENG101", Name = "English", CreditHours = 2, YearId = Year1, SemesterId = Semester1 };
        var math = new Subject { Code = "MTH101", Name = "Math", CreditHours = 4, YearId = Year1, SemesterId = Semester2 };
        var physics = new Subject { Code = "PHY101", Name = "Physics", CreditHours = 3, YearId = Year2, SemesterId = Semester1 };

        db.Subjects.AddRange(english, math, physics);
        db.Offers.AddRange(
            new Offer { SubjectId = english.Id, InstructorId = InstructorA, SemesterId = Semester1 },
            new Offer { SubjectId = math.Id, InstructorId = InstructorA, SemesterId = Semester2 },
            new Offer { SubjectId = physics.Id, InstructorId = InstructorA, SemesterId = Semester1 },
            new Offer { SubjectId = english.Id, InstructorId = InstructorB, SemesterId = Semester1 });
        await db.SaveChangesAsync();
        return db;
    }

    [Fact]
    public async Task Returns_only_the_instructors_offers_ordered_by_subject_name()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorOffersSpecification(InstructorA);

        var subjectNames = await spec.ApplyTo(db.Offers.AsNoTracking())
            .Select(o => o.Subject.Name)
            .ToListAsync();
        Assert.Equal(["English", "Math", "Physics"], subjectNames);
    }

    [Fact]
    public async Task Year_filter_uses_the_subjects_curriculum_year()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorOffersSpecification(InstructorA, yearId: Year1);

        var subjectNames = await spec.ApplyTo(db.Offers.AsNoTracking())
            .Select(o => o.Subject.Name)
            .ToListAsync();
        Assert.Equal(["English", "Math"], subjectNames);
    }

    [Fact]
    public async Task Semester_filter_uses_the_offers_semester()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorOffersSpecification(InstructorA, yearId: Year1, semesterId: Semester1);

        var subjectNames = await spec.ApplyTo(db.Offers.AsNoTracking())
            .Select(o => o.Subject.Name)
            .ToListAsync();
        Assert.Equal(["English"], subjectNames);
    }

    [Fact]
    public async Task Include_loads_the_subject_navigation_for_entity_readers()
    {
        await using var db = await CreateSeededDbAsync();

        var spec = new InstructorOffersSpecification(InstructorB);

        var offers = await spec.ApplyTo(db.Offers.AsNoTracking())
            .ToListAsync();
        var offer = Assert.Single(offers);
        Assert.Equal("English", offer.Subject.Name);
    }
}
