using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class DeleteLectureHandler(
        IApplicationDbContext context, IFileService fileService,
        ILogger<DeleteLectureHandler> logger)
     : IRequestHandler<DeleteLectureCommand, bool>
    {
        public async Task<bool> Handle(DeleteLectureCommand request, CancellationToken cancellationToken)
        {
            var lecture = await context.Lectures.FirstOrDefaultAsync(l => l.Id == request.Id, cancellationToken);
            if (lecture is null)
            {
                logger.LogWarning("Lecture not found for deletion. LectureId: {LectureId}", request.Id);
                return false;
            }

            context.Lectures.Remove(lecture);
            await context.SaveChangesAsync(cancellationToken);

            await fileService.DeleteAsync(lecture.VideoFileId, cancellationToken);
            if (lecture.ThumbnailFileId is not null)
                await fileService.DeleteAsync(lecture.ThumbnailFileId.Value, cancellationToken);

            return true;
        }
    }
}
