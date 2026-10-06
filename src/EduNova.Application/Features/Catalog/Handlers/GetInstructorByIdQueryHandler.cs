using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Queries;
using EduNova.Application.Features.Catalog.Responses;
using EduNova.Application.Features.Catalog.Specifications;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Catalog.Handlers
{
    public sealed class GetInstructorByIdQueryHandler(IApplicationDbContext context)
        : IRequestHandler<GetInstructorByIdQuery, Result<InstructorDetailsResponse>>
    {
        public async Task<Result<InstructorDetailsResponse>> Handle(
            GetInstructorByIdQuery request, CancellationToken cancellationToken)
        {
            var instructor = await context.Instructors
                .AsNoTracking()
                .Where(i => i.Id == request.Id)
                .Select(i => new
                {
                    i.Id,
                    i.FullName,
                    i.AcademicTitle,
                    i.Bio,
                    i.Email,
                    i.ImageUrl,
                    // All years the instructor teaches (drives the year label/tabs).
                    YearsTeaching = i.Offers
                        .Select(o => new InstructorYearResponse(o.Subject.YearId, o.Subject.Year.Name))
                        .Distinct()
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (instructor is null)
                return Result.Failure<InstructorDetailsResponse>(Error.NotFound("Instructor", request.Id));

            // Subject cards narrowed by the screen's year tab + semester toggle —
            // the filtering rules live only in InstructorOffersSpecification.
            var subjects = await new InstructorOffersSpecification(
                    request.Id, request.YearId, request.SemesterId)
                .ApplyTo(context.Offers.AsNoTracking())
                .Select(o => new InstructorSubjectResponse(o.SubjectId, o.Subject.Name))
                .Distinct()
                .ToListAsync(cancellationToken);

            return Result.Success(new InstructorDetailsResponse(
                instructor.Id,
                instructor.FullName,
                instructor.AcademicTitle,
                instructor.Bio,
                instructor.Email,
                instructor.ImageUrl,
                instructor.YearsTeaching,
                subjects));
        }
    }
}
