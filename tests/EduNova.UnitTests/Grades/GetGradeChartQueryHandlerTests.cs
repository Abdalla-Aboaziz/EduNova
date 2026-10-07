using EduNova.Application.Features.Grades.Handlers;
using EduNova.Application.Features.Grades.Queries;
using EduNova.Domain.Entities;
using EduNova.Infrastructure.Data;
using EduNova.UnitTests.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EduNova.UnitTests.Grades;

/// <summary>
/// Chart handler checks: chronological GPA trend, best-first letter
/// distribution, and the same student-scoping security rule as the sheet.
/// </summary>
public class GetGradeChartQueryHandlerTests
{
    private static readonly Guid Year1Id = Guid.NewGuid();
    private static readonly Guid Year2Id = Guid.NewGuid();
    private static readonly Guid Semester1Id = Guid.NewGuid();
    private static readonly Guid Semester2Id = Guid.NewGuid();

    private static ApplicationDbContext CreateSeededDb()
    {
        var db = TestDb.Create();
        db.Years.AddRange(
            new Year { Id = Year1Id, Number = 1, Name = "First Year" },
            new Year { Id = Year2Id, Number = 2, Name = "Second Year" });
        db.Semesters.AddRange(
            new Semester { Id = Semester1Id, YearId = Year1Id, Name = "Semester 1", Order = 1 },
            new Semester { Id = Semester2Id, YearId = Year2Id, Name = "Semester 1", Order = 1 });
        var english = new Subject { Code = "ENG101", Name = "English", CreditHours = 2, YearId = Year1Id, SemesterId = Semester1Id };
        var math = new Subject { Code = "MTH101", Name = "Math", CreditHours = 3, YearId = Year2Id, SemesterId = Semester2Id };
        db.Subjects.AddRange(english, math);
        db.Grades.AddRange(
            new Grade { StudentId = "student-a", SubjectId = english.Id, Score = 27m, MaxScore = 30m },   // A+ 4.0
            new Grade { StudentId = "student-a", SubjectId = math.Id, Score = 24m, MaxScore = 30m },      // B+ 3.3
            new Grade { StudentId = "student-b", SubjectId = english.Id, Score = 10m, MaxScore = 30m });  // F 0.0
        db.SaveChanges();
        return db;
    }

    private static IConfiguration BuildConfig() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["EduNova:DemoStudentId"] = "demo-student-1",
            ["EduNova:DemoStudentName"] = "Mona Ahmed",
        })
        .Build();

    private static GetGradeChartQueryHandler HandlerFor(ApplicationDbContext db, string? userId) =>
        new(db, new FakeCurrentUserService(userId), BuildConfig());

    [Fact]
    public async Task Builds_chronological_gpa_trend_for_the_authenticated_student()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-a")
            .Handle(new GetGradeChartQuery(), CancellationToken.None);

        Assert.Equal(2, result.Value!.GpaTrend.Count);

        var first = result.Value.GpaTrend[0];
        Assert.Equal("Y1-S1", first.Label);
        Assert.Equal("First Year", first.YearName);
        Assert.Equal(4.0m, first.Gpa);       // single A+

        var second = result.Value.GpaTrend[1];
        Assert.Equal("Y2-S1", second.Label);
        Assert.Equal(3.3m, second.Gpa);      // single B+
    }

    [Fact]
    public async Task Distribution_counts_letters_ordered_best_first()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-a")
            .Handle(new GetGradeChartQuery(), CancellationToken.None);

        Assert.Equal(2, result.Value!.GradeDistribution.Count);
        Assert.Equal("A+", result.Value.GradeDistribution[0].Letter);
        Assert.Equal(1, result.Value.GradeDistribution[0].Count);
        Assert.Equal("B+", result.Value.GradeDistribution[1].Letter);
        Assert.Equal(1, result.Value.GradeDistribution[1].Count);
    }

    [Fact]
    public async Task Another_students_grades_never_reach_the_chart()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-b")
            .Handle(new GetGradeChartQuery(), CancellationToken.None);

        var distribution = Assert.Single(result.Value!.GradeDistribution);
        Assert.Equal("F", distribution.Letter);          // only their own failing grade
        Assert.Equal(1, distribution.Count);
        var trendPoint = Assert.Single(result.Value.GpaTrend);
        Assert.Equal(0.0m, trendPoint.Gpa);
    }

    [Fact]
    public async Task Student_with_no_grades_gets_empty_chart()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-with-no-grades")
            .Handle(new GetGradeChartQuery(), CancellationToken.None);

        Assert.Empty(result.Value!.GpaTrend);
        Assert.Empty(result.Value.GradeDistribution);
    }
}
