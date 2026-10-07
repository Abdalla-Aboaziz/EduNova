using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Queries;
using EduNova.Application.Features.Catalog.Responses;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Catalog.Handlers
{
    public sealed class GetSubjectsQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetSubjectsQuery, Result<PagedResult<SubjectListItemResponse>>>
    {
        public async Task<Result<PagedResult<SubjectListItemResponse>>> Handle(
            GetSubjectsQuery request, CancellationToken cancellationToken)
        {
            var spec = new SubjectSearchSpecification(request.Search, request.YearId, request.SemesterId);

            // Spec applies filters + ordering ("what"); the projection shapes the row;
            // ToPagedResultAsync owns count + Skip/Take ("how").
            var page = await spec.ApplyTo(context.Subjects.AsNoTracking())
                .Select(s => new SubjectListItemResponse(
                    s.Id,
                    s.Code,
                    s.Name,
                    s.CreditHours,
                    new SubjectYearResponse(s.YearId, s.Year.Name),
                    new SubjectSemesterResponse(s.SemesterId, s.Semester.Name),
                    s.Offers.Select(o => o.Instructor.FullName).ToList()))
                .ToPagedResultAsync(request, cancellationToken);

            return Result.Success(page);
        }
    }
}
