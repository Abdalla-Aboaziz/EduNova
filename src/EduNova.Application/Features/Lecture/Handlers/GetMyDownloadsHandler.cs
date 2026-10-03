using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetMyDownloadsHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
        : IRequestHandler<GetMyDownloadsQuery, List<DownloadResponse>>
    {
        public Task<List<DownloadResponse>> Handle(GetMyDownloadsQuery request, CancellationToken cancellationToken)
        {
            var userId = "101";//_currentUserService.UserId;  ToDo: Get the current user ID from the ICurrentUserService

            var downloads = context.DownloadedLectures
             .AsNoTracking()
             .Where(d => d.UserId == userId)
             .OrderByDescending(d => d.DownloadedAt)
             .Join(context.Lectures,
                   d => d.LectureId,
                   l => l.Id,
                   (d, l) => new DownloadResponse
                   (
                      l.Id,
                       l.Title,
                      l.DurationInSeconds,
                       d.DownloadedAt
                   )).ToListAsync(cancellationToken);
            return downloads;
        }
    }
}
