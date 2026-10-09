using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Book.Queries;
using EduNova.Application.Features.Book.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Book.Handlers;

public sealed class GetBooksQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBooksQuery, Result<PagedResult<BookListItemResponse>>>
{
    public async Task<Result<PagedResult<BookListItemResponse>>> Handle(
        GetBooksQuery request, CancellationToken cancellationToken)
    {
        var page = await context.Books
            .AsNoTracking()
            .Where(b => request.SubjectId == null || b.SubjectId == request.SubjectId)
            .Where(b => string.IsNullOrWhiteSpace(request.Search)
                || b.Title.Contains(request.Search)
                || (b.Description != null && b.Description.Contains(request.Search)))
            .OrderBy(b => b.Title)
            .Select(b => new BookListItemResponse(
                b.Id,
                b.Title,
                b.Description,
                b.SubjectId,
                b.Subject != null ? b.Subject.Name : null,
                b.CreatedAt))
            .ToPagedResultAsync(request, cancellationToken);

        return Result.Success(page);
    }
}