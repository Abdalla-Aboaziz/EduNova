using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Queries;
using EduNova.Application.Features.Catalog.Responses;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Catalog.Handlers
{
    public sealed class GetHomeSummaryQueryHandler(
        IApplicationDbContext context,
        ICurrentUserService currentUser,
        IConfiguration configuration)
        : IRequestHandler<GetHomeSummaryQuery, Result<HomeSummaryResponse>>
    {
        public async Task<Result<HomeSummaryResponse>> Handle(
            GetHomeSummaryQuery request, CancellationToken cancellationToken)
        {
            // TODO(AUTH): drop the demo fallback once member 1's JWT lands —
            // the id/name will come from the authenticated user's claims.
            var studentId = currentUser.UserId
                ?? configuration["EduNova:DemoStudentId"]
                ?? "demo-student-1";
            var fullName = configuration["EduNova:DemoStudentName"] ?? "Demo Student";

            var yearNumber = ParseInt(configuration["EduNova:CurrentYearNumber"], 4);
            var semesterOrder = ParseInt(configuration["EduNova:CurrentSemesterOrder"], 1);

            var currentTerm = await context.Semesters
                .AsNoTracking()
                .Where(s => s.Year.Number == yearNumber && s.Order == semesterOrder)
                .Select(s => new { s.Id, s.Name, s.YearId, YearName = s.Year.Name })
                .FirstOrDefaultAsync(cancellationToken);

            if (currentTerm is null)
                return Result.Failure<HomeSummaryResponse>(Error.Failure(
                    "CURRENT_TERM_NOT_CONFIGURED",
                    $"No semester matches year {yearNumber} and semester order {semesterOrder}. " +
                    "Check the EduNova section in appsettings."));

            // Current-term subjects via the shared spec — one roundtrip with the
            // lecture count as a correlated subquery (no cross-module FK, D6).
            var spec = new SubjectSearchSpecification(null, currentTerm.YearId, currentTerm.Id);

            var subjects = await spec.ApplyTo(context.Subjects.AsNoTracking())
                .Select(s => new HomeSubjectResponse(
                    s.Id,
                    s.Name,
                    s.Offers.Select(o => o.Instructor.FullName).FirstOrDefault(),
                    s.CreditHours,
                    context.Lectures.Count(l => l.SubjectId == s.Id)))
                .ToListAsync(cancellationToken);

            return Result.Success(new HomeSummaryResponse(
                new HomeYearResponse(currentTerm.YearId, currentTerm.YearName),
                new HomeSemesterResponse(currentTerm.Id, currentTerm.Name),
                new HomeStudentResponse(studentId, FirstName(fullName), fullName, Gpa: null), // GPA lands with T4.4
                subjects));
        }

        private static string FirstName(string fullName) => fullName.Split(' ', 2)[0];

        private static int ParseInt(string? value, int fallback) =>
            int.TryParse(value, out var parsed) ? parsed : fallback;
    }
}
