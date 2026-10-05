using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace EduNova.Application.Common.Pagination;

public static class QueryableExtensions
{
    /// <summary>
    /// Counts the full result set, then materializes only the requested page.
    /// Both the count and the page queries run server-side — apply filtering
    /// and ordering to the IQueryable before calling this.
    /// </summary>
    public static async Task<PagedResult<T>> ToPagedResultAsync<T>(
        this IQueryable<T> query,
        PagedRequest request,
        CancellationToken cancellationToken = default)
    {
        // EF providers run both queries asynchronously on the server;
        // plain LINQ-to-Objects providers have no async story, fall back to sync.
        if (query.Provider is IAsyncQueryProvider)
        {
            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            return new PagedResult<T>
            {
                Items = items,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalCount = totalCount
            };
        }

        return new PagedResult<T>
        {
            Items = query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList(),
            Page = request.Page,
            PageSize = request.PageSize,
            TotalCount = query.Count()
        };
    }
}
