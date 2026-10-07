using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Responses;
using MediatR;

namespace EduNova.Application.Features.Catalog.Queries
{
    /// <summary>
    /// Paged instructor list for the Instructors screen: optional search by
    /// full name.
    /// </summary>
    public class GetInstructorsQuery : PagedRequest, IRequest<Result<PagedResult<InstructorListItemResponse>>>
    {
        public string? Search { get; set; }
    }
}
