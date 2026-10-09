using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetLectureThumbnailHandler(
        IApplicationDbContext context, IFileService fileService,
        ILogger<GetLectureThumbnailHandler> logger)
    : IRequestHandler<GetLectureThumbnailQuery, (Stream? Stream, string ContentType, string FileName)>
    {
        public async Task<(Stream? Stream, string ContentType, string FileName)> Handle(
            GetLectureThumbnailQuery request, CancellationToken cancellationToken)
        {
            var thumbnailId = await context.Lectures
                .AsNoTracking()
                .Where(l => l.Id == request.Id)
                .Select(l => l.ThumbnailFileId)
                .FirstOrDefaultAsync(cancellationToken);

            if (thumbnailId is null)
            {
                logger.LogWarning("Thumbnail not found for LectureId {LectureId}", request.Id);
                return (null, string.Empty, string.Empty);
            }

            return await fileService.StreamAsync(thumbnailId.Value, cancellationToken);
        }
    }
}
