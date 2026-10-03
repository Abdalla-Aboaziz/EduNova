using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class RegisterDownloadCommandHandler(
        IApplicationDbContext context, ICurrentUserService currentUserService,
        ILogger<RegisterDownloadCommandHandler> logger)
        : IRequestHandler<RegisterDownloadCommand, bool>
    {
        public async Task<bool> Handle(RegisterDownloadCommand request, CancellationToken cancellationToken)
        {
            var userId = "101";//_currentUserService.UserId; TODO: Implement user authentication and get the actual user ID

            if (!await context.Lectures.AnyAsync(l => l.Id == request.LectureId, cancellationToken))
            {
                logger.LogWarning("Lecture not found for download registration. UserId: {UserId}, LectureId: {LectureId}", userId, request.LectureId);
                return false;
            }

            var exists = await context.DownloadedLectures.AnyAsync(ld => ld.LectureId == request.LectureId && ld.UserId == userId, cancellationToken);

            if (!exists)
            {
                var downloadedLecture = new DownloadedLecture
                {
                    LectureId = request.LectureId,
                    UserId = userId
                };
                context.DownloadedLectures.Add(downloadedLecture);
                await context.SaveChangesAsync(cancellationToken);
            }

            return true;
        }
    }
}
