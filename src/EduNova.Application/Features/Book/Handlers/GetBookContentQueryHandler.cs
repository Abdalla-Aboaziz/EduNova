using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Book.Queries;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Book.Handlers;

public sealed class GetBookContentQueryHandler(
    IApplicationDbContext context,
    IFileService fileService,
    ILogger<GetBookContentQueryHandler> logger)
    : IRequestHandler<GetBookContentQuery, (Stream? stream, string ContentType, string FileName)>
{
    public async Task<(Stream? stream, string ContentType, string FileName)> Handle(
        GetBookContentQuery request, CancellationToken cancellationToken)
    {
        var fileId = await context.Books
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .Select(b => (Guid?)b.FileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (fileId is null)
        {
            logger.LogWarning("Book or book file not found for reading. BookId: {BookId}", request.Id);
            return (null, string.Empty, string.Empty);
        }

        return await fileService.StreamAsync(fileId.Value, cancellationToken);
    }
}