using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Catalog.Responses;
using MediatR;

namespace EduNova.Application.Features.Catalog.Queries
{
    /// <summary>
    /// Paged subject list for the Filter screen: search by name or code with
    /// optional year and semester filters. Inherits PagedRequest so
    /// ?page=&pageSize= binds flat from the query string.
    /// </summary>
    public class GetSubjectsQuery : PagedRequest, IRequest<Result<PagedResult<SubjectListItemResponse>>>
    {
        public string? Search { get; set; }
        public Guid? YearId { get; set; }
        public Guid? SemesterId { get; set; }
    }
}
