using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Book.Responses;
using MediatR;

namespace EduNova.Application.Features.Book.Queries;

public class GetBooksQuery : PagedRequest, IRequest<Result<PagedResult<BookListItemResponse>>>
{
    public string? Search { get; set; }
    public Guid? SubjectId { get; set; }
}