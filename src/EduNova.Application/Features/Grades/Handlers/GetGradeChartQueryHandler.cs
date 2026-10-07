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
    public sealed class GetGradeChartQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IConfiguration configuration)
        : IRequestHandler<GetGradeChartQuery, Result<GradeChartResponse>>
    {
        public async Task<Result<GradeChartResponse>> Handle(
            GetGradeChartQuery request, CancellationToken cancellationToken)
        {
            // SECURITY: same rule as the grade sheet — the student is always the
            // resolved identity; studentId is not part of the request.
            // TODO(AUTH): drop the demo fallback once member 1's JWT lands.
            var studentId = currentUser.UserId
                ?? configuration["EduNova:DemoStudentId"]
                ?? "demo-student-1";

            var spec = new GradeSheetSpecification(studentId);

            var grades = await spec.ApplyTo(context.Grades.AsNoTracking())
                .Select(g => new
                {
                    g.Subject.CreditHours,
                    g.Score,
                    g.MaxScore,
                    g.Subject.YearId,
                    YearName = g.Subject.Year.Name,
                    YearNumber = g.Subject.Year.Number,
                    g.Subject.SemesterId,
                    SemesterName = g.Subject.Semester.Name,
                    SemesterOrder = g.Subject.Semester.Order
                })
                .ToListAsync(cancellationToken);

            var gpaTrend = grades
                .GroupBy(g => new
                {
                    g.YearId,
                    g.YearName,
                    g.YearNumber,
                    g.SemesterId,
                    g.SemesterName,
                    g.SemesterOrder
                })
                .Select(termGroup => new GradeChartTermPoint(
                    $"Y{termGroup.Key.YearNumber}-S{termGroup.Key.SemesterOrder}",
                    termGroup.Key.YearId,
                    termGroup.Key.SemesterId,
                    termGroup.Key.YearName,
                    termGroup.Key.SemesterName,
                    GradeCalculator.CalculateGpa(
                        termGroup.Select(g => (g.Score, g.MaxScore, g.CreditHours)))))
                .ToList();

            var distribution = grades
                .Select(g =>
                {
                    var percent = GradeCalculator.ToPercent(g.Score, g.MaxScore);
                    return (Letter: GradeCalculator.ToLetter(percent),
                            GradePoint: GradeCalculator.ToGradePoint(percent));
                })
                .GroupBy(x => x.Letter)
                .Select(group => new
                {
                    Letter = group.Key,
                    group.First().GradePoint,
                    Count = group.Count()
                })
                .OrderByDescending(x => x.GradePoint)   // best letter first
                .Select(x => new GradeChartLetterCount(x.Letter, x.Count))
                .ToList();

            return Result.Success(new GradeChartResponse(gpaTrend, distribution));
        }
    }
}
