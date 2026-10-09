using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Book.Queries;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Book.Handlers;

public sealed class DownloadBookContentQueryHandler(
    IApplicationDbContext context,
    IFileService fileService)
    : IRequestHandler<DownloadBookContentQuery, (byte[] fileContent, string contentType, string fileName)>
{
    public async Task<(byte[] fileContent, string contentType, string fileName)> Handle(
        DownloadBookContentQuery request, CancellationToken cancellationToken)
    {
        var fileId = await context.Books
            .AsNoTracking()
            .Where(b => b.Id == request.Id)
            .Select(b => (Guid?)b.FileId)
            .FirstOrDefaultAsync(cancellationToken);

        if (fileId is null)
            return (Array.Empty<byte>(), string.Empty, string.Empty);

        return await fileService.DownloadAsync(fileId.Value, cancellationToken);
    }
}