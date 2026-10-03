using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class DownloadLectureQueryHandler(
        IApplicationDbContext context, IFileService fileService,
        ICurrentUserService currentUser,
        ILogger<DownloadLectureQueryHandler> logger)
        : IRequestHandler<DownloadLectureQuery, (byte[] FileContent, string ContentType, string FileName)?>
    {
        public async Task<(byte[] FileContent, string ContentType, string FileName)?> Handle(
            DownloadLectureQuery request, CancellationToken cancellationToken)
        {
            var userId = "101"; // TODO: Replace with currentUser.UserId when authentication is implemented

            logger.LogInformation("User {UserId} downloading LectureId {LectureId}", userId, request.LectureId);

            var videoFileId = await context.Lectures
                .AsNoTracking()
                .Where(l => l.Id == request.LectureId)
                .Select(l => (Guid?)l.VideoFileId)
                .FirstOrDefaultAsync(cancellationToken);

            if (videoFileId is null)
            {
                logger.LogWarning("Lecture not found for download. LectureId: {LectureId}", request.LectureId);
                return null;
            }

            var (fileContent, contentType, fileName) = await fileService.DownloadAsync(videoFileId.Value, cancellationToken);

            if (fileContent.Length == 0)
            {
                logger.LogWarning("Video file not found on disk. LectureId: {LectureId}, VideoFileId: {VideoFileId}", request.LectureId, videoFileId);
                return null;
            }

            // Register the download for tracking
            var alreadyDownloaded = await context.DownloadedLectures
                .AnyAsync(d => d.LectureId == request.LectureId && d.UserId == userId, cancellationToken);

            if (!alreadyDownloaded)
            {
                context.DownloadedLectures.Add(new DownloadedLecture
                {
                    LectureId = request.LectureId,
                    UserId = userId
                });
                await context.SaveChangesAsync(cancellationToken);
            }

            logger.LogInformation(
                "Lecture downloaded successfully. UserId: {UserId}, LectureId: {LectureId}, FileSize: {FileSize} bytes",
                userId, request.LectureId, fileContent.Length);

            return (fileContent, contentType, fileName);
        }
    }
}
