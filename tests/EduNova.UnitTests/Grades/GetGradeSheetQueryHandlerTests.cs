using EduNova.Application.Features.Grades.Handlers;
using EduNova.Application.Features.Grades.Queries;
using EduNova.Domain.Entities;
using EduNova.Infrastructure.Data;
using EduNova.UnitTests.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EduNova.UnitTests.Grades;

/// <summary>
/// Grade Sheet handler checks — most importantly the security rule
/// (requirement 9): the sheet is scoped to the resolved student only; a
/// client can never reach another student's grades because studentId is not
/// part of the request at all.
/// </summary>
public class GetGradeSheetQueryHandlerTests
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
            // student-a: 27/30 (90% -> A+ 4.0) and 24/30 (80% -> B+ 3.3)
            new Grade { StudentId = "student-a", SubjectId = english.Id, Score = 27m, MaxScore = 30m },
            new Grade { StudentId = "student-a", SubjectId = math.Id, Score = 24m, MaxScore = 30m },
            // student-b on the same subject — must never leak into student-a's sheet
            new Grade { StudentId = "student-b", SubjectId = english.Id, Score = 10m, MaxScore = 30m });
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

    private static GetGradeSheetQueryHandler HandlerFor(ApplicationDbContext db, string? userId) =>
        new(db, new FakeCurrentUserService(userId), BuildConfig());

    [Fact]
    public async Task Returns_only_the_authenticated_students_grades()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-a")
            .Handle(new GetGradeSheetQuery(null, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        var subjects = result.Value!.Terms.SelectMany(t => t.Subjects).ToList();
        Assert.Equal(2, subjects.Count);                        // student-b's row never appears
        Assert.DoesNotContain(subjects, s => s.Score == 10m);   // student-b's failing grade
        Assert.Equal(["English", "Math"], subjects.Select(s => s.Name).ToList());
    }

    [Fact]
    public async Task Groups_terms_chronologically_with_term_and_overall_gpa()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-a")
            .Handle(new GetGradeSheetQuery(null, null), CancellationToken.None);

        Assert.Equal(2, result.Value!.Terms.Count);

        var first = result.Value.Terms[0];
        Assert.Equal("First Year", first.YearName);
        var english = Assert.Single(first.Subjects);
        Assert.Equal("27/30", english.ScoreText);
        Assert.Equal(90m, english.Percent);
        Assert.Equal("A+", english.Letter);      // 90% on the faculty scale
        Assert.Equal(4.0m, english.GradePoint);
        Assert.Equal(4.0m, first.TermGpa);

        var second = result.Value.Terms[1];
        var math = Assert.Single(second.Subjects);
        Assert.Equal("B+", math.Letter);         // 80% on the faculty scale
        Assert.Equal(3.3m, second.TermGpa);

        Assert.Equal(3.58m, result.Value.Student.OverallGpa);   // (4.0×2 + 3.3×3) / 5
        Assert.Equal("Mona", result.Value.Student.FirstName);
        Assert.Equal("Mona Ahmed", result.Value.Student.FullName);
    }

    [Fact]
    public async Task Year_filter_keeps_only_that_terms()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-a")
            .Handle(new GetGradeSheetQuery(Year1Id, null), CancellationToken.None);

        var term = Assert.Single(result.Value!.Terms);
        Assert.Equal("First Year", term.YearName);
        Assert.Equal("English", Assert.Single(term.Subjects).Name);
    }

    [Fact]
    public async Task Unauthenticated_requests_fall_back_to_the_demo_student()
    {
        await using var db = TestDb.Create();
        db.Years.Add(new Year { Id = Year1Id, Number = 1, Name = "First Year" });
        db.Semesters.Add(new Semester { Id = Semester1Id, YearId = Year1Id, Name = "Semester 1", Order = 1 });
        var subject = new Subject { Code = "ENG101", Name = "English", CreditHours = 2, YearId = Year1Id, SemesterId = Semester1Id };
        db.Subjects.Add(subject);
        db.Grades.Add(new Grade { StudentId = "demo-student-1", SubjectId = subject.Id, Score = 27m, MaxScore = 30m });
        db.SaveChanges();

        var result = await HandlerFor(db, null)   // no authenticated user
            .Handle(new GetGradeSheetQuery(null, null), CancellationToken.None);

        var subjectResponse = Assert.Single(result.Value!.Terms.SelectMany(t => t.Subjects));
        Assert.Equal("English", subjectResponse.Name);
    }

    [Fact]
    public async Task Student_with_no_grades_gets_an_empty_sheet_not_an_error()
    {
        await using var db = CreateSeededDb();

        var result = await HandlerFor(db, "student-with-no-grades")
            .Handle(new GetGradeSheetQuery(null, null), CancellationToken.None);

        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!.Terms);
        Assert.Null(result.Value.Student.OverallGpa);
    }
}
