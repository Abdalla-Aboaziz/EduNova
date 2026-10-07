using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Grades.Queries;
using EduNova.Application.Features.Grades.Responses;
using EduNova.Application.Features.Grades.Specifications;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Grades.Handlers
{
    public sealed class GetGradeSheetQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IConfiguration configuration)
        : IRequestHandler<GetGradeSheetQuery, Result<GradeSheetResponse>>
    {
        public async Task<Result<GradeSheetResponse>> Handle(
            GetGradeSheetQuery request, CancellationToken cancellationToken)
        {
            // SECURITY: the student is ALWAYS the resolved identity — a client
            // cannot ask for another student's grades because studentId is not
            // part of the request at all.
            // TODO(AUTH): drop the demo fallback once member 1's JWT lands.
            var studentId = currentUser.UserId
                ?? configuration["EduNova:DemoStudentId"]
                ?? "demo-student-1";
            var fullName = configuration["EduNova:DemoStudentName"] ?? "Demo Student";

            var spec = new GradeSheetSpecification(studentId, request.YearId, request.SemesterId);

            var grades = await spec.ApplyTo(context.Grades.AsNoTracking())
                .Select(g => new
                {
                    g.SubjectId,
                    SubjectName = g.Subject.Name,
                    g.Subject.CreditHours,
                    g.Subject.YearId,
                    YearName = g.Subject.Year.Name,
                    g.Subject.SemesterId,
                    SemesterName = g.Subject.Semester.Name,
                    g.Score,
                    g.MaxScore,
                    InstructorName = g.Offer != null ? g.Offer.Instructor.FullName : null,
                    g.ExamAt
                })
                .ToListAsync(cancellationToken);

            var terms = grades
                .GroupBy(g => new { g.YearId, g.YearName, g.SemesterId, g.SemesterName })
                .Select(termGroup =>
                {
                    var subjects = termGroup
                        .Select(g =>
                        {
                            var percent = GradeCalculator.ToPercent(g.Score, g.MaxScore);
                            return new GradeSheetSubjectResponse(
                                g.SubjectId,
                                g.SubjectName,
                                g.CreditHours,
                                g.Score,
                                g.MaxScore,
                                $"{g.Score:0.##}/{g.MaxScore:0.##}",
                                percent,
                                GradeCalculator.ToLetter(percent),
                                GradeCalculator.ToGradePoint(percent),
                                g.InstructorName,
                                g.ExamAt);
                        })
                        .ToList();

                    var termGpa = GradeCalculator.CalculateGpa(
                        subjects.Select(s => (s.GradePoint, s.CreditHours)));

                    return new GradeSheetTermResponse(
                        termGroup.Key.YearId,
                        termGroup.Key.YearName,
                        termGroup.Key.SemesterId,
                        termGroup.Key.SemesterName,
                        termGpa,
                        subjects);
                })
                .ToList();

            var overallGpa = GradeCalculator.CalculateGpa(
                terms.SelectMany(t => t.Subjects).Select(s => (s.GradePoint, s.CreditHours)));

            return Result.Success(new GradeSheetResponse(
                new GradeSheetStudentResponse(FirstName(fullName), fullName, overallGpa),
                terms));
        }

        private static string FirstName(string fullName) => fullName.Split(' ', 2)[0];
    }
}
