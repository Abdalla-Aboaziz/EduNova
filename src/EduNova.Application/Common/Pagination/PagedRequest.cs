namespace EduNova.Application.Common.Pagination;

/// <summary>
/// Query-string paging input. Paginated queries inherit from it so that
/// ?page=2&amp;pageSize=20 binds flat in [FromQuery] endpoints.
/// </summary>
public class PagedRequest
{
    public const int DefaultPageSize = 10;
    public const int MaxPageSize = 50;

    public int Page { get; set; } = 1;

    public int PageSize { get; set; } = DefaultPageSize;
}
