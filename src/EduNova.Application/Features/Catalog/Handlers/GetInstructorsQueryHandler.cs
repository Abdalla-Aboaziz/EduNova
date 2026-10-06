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
    public sealed class GetInstructorsQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetInstructorsQuery, Result<PagedResult<InstructorListItemResponse>>>
    {
        public async Task<Result<PagedResult<InstructorListItemResponse>>> Handle(
            GetInstructorsQuery request, CancellationToken cancellationToken)
        {
            var spec = new InstructorSearchSpecification(request.Search);

            var page = await spec.ApplyTo(context.Instructors.AsNoTracking())
                .Select(i => new InstructorListItemResponse(
                    i.Id,
                    i.FullName,
                    i.AcademicTitle,
                    i.ImageUrl,
                    i.Offers.Select(o => o.SubjectId).Distinct().Count()))
                .ToPagedResultAsync(request, cancellationToken);

            return Result.Success(page);
        }
    }
}
