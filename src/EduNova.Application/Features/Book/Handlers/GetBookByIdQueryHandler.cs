using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Book.Queries;
using EduNova.Application.Features.Book.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Book.Handlers;

public sealed class GetBookByIdQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetBookByIdQuery, Result<BookResponse>>
{
    public async Task<Result<BookResponse>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var book = await context.Books
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .Select(b => new BookResponse(
                b.Id,
                b.Title,
                b.Description,
                b.SubjectId,
                b.Subject != null ? b.Subject.Name : null,
                b.FileId,
                b.File != null ? b.File.FileName : string.Empty,
                b.File != null ? b.File.ContentType : string.Empty,
                b.File != null ? b.File.FileSize : 0))
            .FirstOrDefaultAsync(cancellationToken);

        return book is null
            ? Result.Failure<BookResponse>(Error.NotFound("Book", request.Id))
            : Result.Success(book);
    }
}