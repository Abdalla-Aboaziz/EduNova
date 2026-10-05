namespace EduNova.Application.Common.Pagination;

/// <summary>
/// One page of results plus the metadata clients need for the paging UI.
/// Serialized at the top level of list responses.
/// </summary>
public class PagedResult<T>
{
    public IReadOnlyList<T> Items { get; init; } = [];

    public int Page { get; init; }

    public int PageSize { get; init; }

    public int TotalCount { get; init; }

    public int TotalPages => PageSize <= 0
        ? 0
        : (int)Math.Ceiling(TotalCount / (double)PageSize);

    public bool HasNext => Page < TotalPages;

    public bool HasPrevious => Page > 1;
}
