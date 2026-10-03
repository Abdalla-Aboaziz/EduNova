using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class UploadLectureHandler(
     IApplicationDbContext context, IFileService fileService, ICurrentUserService currentUser,
     ILogger<UploadLectureHandler> logger)
     : IRequestHandler<UploadLectureCommand, Guid>
    {
        public async Task<Guid> Handle(UploadLectureCommand request, CancellationToken cancellationToken)
        {
            var videoId = await fileService.UploadAsync(request.Video, cancellationToken);
            Guid? thumbnailId = null;

            try
            {
                if (request.Thumbnail is not null)
                {
                    thumbnailId = await fileService.UploadAsync(request.Thumbnail, cancellationToken);
                }

                var lecture = new EduNova.Domain.Entities.Lecture
                {
                    SubjectId = request.SubjectId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = request.Order,
                    DurationInSeconds = request.DurationInSeconds,
                    VideoFileId = videoId,
                    ThumbnailFileId = thumbnailId,
                    UploadedById = "100",      /// To Do Replase The "100" with currentUser.UserId when authentication is implemented
                };

                await context.Lectures.AddAsync(lecture, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                return lecture.Id;
            }
            catch (Exception ex)
            {
                logger.LogError(ex,
                    "Failed to upload lecture '{Title}' for SubjectId {SubjectId}. Cleaning up uploaded files. VideoFileId: {VideoFileId}, ThumbnailFileId: {ThumbnailFileId}",
                    request.Title, request.SubjectId, videoId, thumbnailId);

                await fileService.DeleteAsync(videoId, cancellationToken);
                if (thumbnailId is not null)
                    await fileService.DeleteAsync(thumbnailId.Value, cancellationToken);
                throw;
            }
        }
    }

}
