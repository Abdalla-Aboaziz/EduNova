using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Responses;
using MediatR;

namespace EduNova.Application.Features.Catalog.Queries
{
    /// <summary>
    /// Instructor details for the Dr. Details screen. yearId/semesterId come
    /// from the year tabs and semester toggle and narrow the subjects list;
    /// they never filter the instructor itself.
    /// </summary>
    public class GetInstructorByIdQuery(Guid id, Guid? yearId, Guid? semesterId)
        : IRequest<Result<InstructorDetailsResponse>>
    {
        public Guid Id { get; set; } = id;
        public Guid? YearId { get; set; } = yearId;
        public Guid? SemesterId { get; set; } = semesterId;
    }
}
