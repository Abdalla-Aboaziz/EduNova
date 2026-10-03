using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using MediatR;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class UploadLectureHandler(
     IApplicationDbContext context, IFileService fileService, ICurrentUserService currentUser)
     : IRequestHandler<UploadLectureCommand, Guid>
    {
        public async Task<Guid> Handle(UploadLectureCommand request, CancellationToken cancellationToken)
        {
            var videoId = await fileService.UploadAsync(request.Video, cancellationToken);

            try
            {
                var lecture = new EduNova.Domain.Entities.Lecture
                {
                    SubjectId = request.SubjectId,
                    Title = request.Title,
                    Description = request.Description,
                    Order = request.Order,
                    DurationInSeconds = request.DurationInSeconds,
                    VideoFileId = videoId,
                    UploadedById = "100",      /// To Do Replase The "100" with currentUser.UserId when authentication is implemented
                };

                await context.Lectures.AddAsync(lecture, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);
                return lecture.Id;
            }
            catch
            {
                await fileService.DeleteAsync(videoId, cancellationToken);
                throw;
            }
        }
    }
}
