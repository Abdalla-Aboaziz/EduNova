using EduNova.Application.Common.Specifications;
using EduNova.Domain.Entities;

namespace EduNova.Application.Features.Grades.Specifications
{
    /// <summary>
    /// Grades of ONE student for the Grade Sheet screen. The studentId must
    /// always come from the resolved identity (ICurrentUserService / demo
    /// fallback) — never from the client request. Optional year and semester
    /// filters follow the subject's curriculum placement. Ordering drives the
    /// per-term grouping: year, then semester, then subject name.
    /// </summary>
    public sealed class GradeSheetSpecification : Specification<Grade>
    {
        public GradeSheetSpecification(string studentId, Guid? yearId = null, Guid? semesterId = null)
        {
            Where(g => g.StudentId == studentId);

            if (yearId is not null)
                Where(g => g.Subject.YearId == yearId);

            if (semesterId is not null)
                Where(g => g.Subject.SemesterId == semesterId);

            OrderBy(g => g.Subject.Year.Number);
            ThenBy(g => g.Subject.Semester.Order);
            ThenBy(g => g.Subject.Name);
        }
    }
}
