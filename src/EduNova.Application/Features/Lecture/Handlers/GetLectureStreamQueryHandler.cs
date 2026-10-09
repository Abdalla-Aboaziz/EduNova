using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetLectureStreamQueryHandler(
        IApplicationDbContext context, IFileService fileService,
        ILogger<GetLectureStreamQueryHandler> logger)
        : IRequestHandler<GetLectureStreamQuery, (FileStream? stream, string ContentType, string FileName)>
    {
        public async Task<(FileStream? stream, string ContentType, string FileName)> Handle(
            GetLectureStreamQuery request, CancellationToken cancellationToken)
        {
            var videoFileId = await context.Lectures
             .AsNoTracking()
             .Where(l => l.Id == request.Id)
             .Select(l => (Guid?)l.VideoFileId)
             .FirstOrDefaultAsync(cancellationToken);

            if (videoFileId is null)
            {
                logger.LogWarning("Lecture or video file not found for streaming. LectureId: {LectureId}", request.Id);
                return (null, string.Empty, string.Empty);
            }

            return await fileService.StreamAsync(videoFileId.Value, cancellationToken);
        }
    }
}
