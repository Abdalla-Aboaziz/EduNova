using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Queries;
using EduNova.Application.Features.Catalog.Responses;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Catalog.Handlers
{
    public sealed class GetSubjectByIdQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetSubjectByIdQuery, Result<SubjectDetailsResponse>>
    {
        public async Task<Result<SubjectDetailsResponse>> Handle(
            GetSubjectByIdQuery request, CancellationToken cancellationToken)
        {
            var spec = new SubjectByIdSpecification(request.Id);

            // One roundtrip: subject + its year/semester + instructors from the
            // offers + lecture count as a correlated subquery (no FK, decision D6).
            var subject = await spec.ApplyTo(context.Subjects.AsNoTracking())
                .Select(s => new SubjectDetailsResponse(
                    s.Id,
                    s.Code,
                    s.Name,
                    s.Description,
                    s.CreditHours,
                    new SubjectYearResponse(s.YearId, s.Year.Name),
                    new SubjectSemesterResponse(s.SemesterId, s.Semester.Name),
                    s.Offers
                        .Select(o => new SubjectInstructorResponse(
                            o.InstructorId,
                            o.Instructor.FullName,
                            o.Instructor.AcademicTitle,
                            o.Instructor.ImageUrl))
                        .ToList(),
                    context.Lectures.Count(l => l.SubjectId == s.Id)))
                .FirstOrDefaultAsync(cancellationToken);

            if (subject is null)
                return Result.Failure<SubjectDetailsResponse>(Error.NotFound("Subject", request.Id));

            return Result.Success(subject);
        }
    }
}
