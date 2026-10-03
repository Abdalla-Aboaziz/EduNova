using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class RemoveDownloadCommandHandler(
        IApplicationDbContext context, ICurrentUserService currentUser,
        ILogger<RemoveDownloadCommandHandler> logger)
        : IRequestHandler<RemoveDownloadCommand, bool>
    {
        public async Task<bool> Handle(RemoveDownloadCommand request, CancellationToken cancellationToken)
        {
            var userId = "101";//_currentUser.UserId; // TODO: Implement user authentication and get the current user ID

            var deleted = await context.DownloadedLectures
                .Where(x => x.UserId == userId && x.LectureId == request.LectureId)
                .ExecuteDeleteAsync(cancellationToken);

            if (deleted == 0)
                logger.LogWarning("Download record not found. UserId: {UserId}, LectureId: {LectureId}", userId, request.LectureId);

            return deleted > 0;
        }
    }
}
