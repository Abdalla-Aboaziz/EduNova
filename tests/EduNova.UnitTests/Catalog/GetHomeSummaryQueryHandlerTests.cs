using EduNova.Application.Features.Catalog.Handlers;
using EduNova.Application.Features.Catalog.Queries;
using EduNova.Domain.Entities;
using EduNova.Infrastructure.Data;
using EduNova.UnitTests.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace EduNova.UnitTests.Catalog;

/// <summary>
/// Home summary wiring from T4.4: the student GPA now comes from the grades
/// table via GradeCalculator (null while the student has no grades).
/// </summary>
public class GetHomeSummaryQueryHandlerTests
{
    private static IConfiguration BuildConfig() => new ConfigurationBuilder()
        .AddInMemoryCollection(new Dictionary<string, string?>
        {
            ["EduNova:DemoStudentId"] = "demo-student-1",
            ["EduNova:DemoStudentName"] = "Mona Ahmed",
            ["EduNova:CurrentYearNumber"] = "4",
            ["EduNova:CurrentSemesterOrder"] = "1",
        })
        .Build();

    private static GetHomeSummaryQueryHandler HandlerFor(ApplicationDbContext db, string? userId) =>
        new(db, new FakeCurrentUserService(userId), BuildConfig());

    [Fact]
    public async Task Home_gpa_is_computed_from_the_students_grades()
    {
        await using var db = TestDb.Create();
        var year = new Year { Id = Guid.NewGuid(), Number = 4, Name = "Fourth Year" };
        var semester = new Semester { Id = Guid.NewGuid(), YearId = year.Id, Name = "Semester 1", Order = 1 };
        var subject = new Subject { Code = "AII401", Name = "Artificial Intelligence", CreditHours = 3, YearId = year.Id, SemesterId = semester.Id };
        db.Years.Add(year);
        db.Semesters.Add(semester);
        db.Subjects.Add(subject);
        db.Grades.Add(new Grade { StudentId = "demo-student-1", SubjectId = subject.Id, Score = 27m, MaxScore = 30m });   // A+ 4.0
        db.SaveChanges();

        var result = await HandlerFor(db, null)
            .Handle(new GetHomeSummaryQuery(), CancellationToken.None);

        Assert.Equal(4.0m, result.Value!.Student.Gpa);
        Assert.Equal("Artificial Intelligence", Assert.Single(result.Value.MySubjects).Name);
    }

    [Fact]
    public async Task Home_gpa_stays_null_when_the_student_has_no_grades()
    {
        await using var db = TestDb.Create();
        var year = new Year { Id = Guid.NewGuid(), Number = 4, Name = "Fourth Year" };
        var semester = new Semester { Id = Guid.NewGuid(), YearId = year.Id, Name = "Semester 1", Order = 1 };
        db.Years.Add(year);
        db.Semesters.Add(semester);
        db.Subjects.Add(new Subject { Code = "AII401", Name = "Artificial Intelligence", CreditHours = 3, YearId = year.Id, SemesterId = semester.Id });
        db.SaveChanges();

        var result = await HandlerFor(db, null)
            .Handle(new GetHomeSummaryQuery(), CancellationToken.None);

        Assert.Null(result.Value!.Student.Gpa);
        Assert.Single(result.Value.MySubjects);
    }
}
