using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetSubjectLecturesQueryHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
        : IRequestHandler<GetSubjectLecturesQuery, List<LectureResponse>>
    {
        public async Task<List<LectureResponse>> Handle(GetSubjectLecturesQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            var lectures = await context.Lectures
                 .AsNoTracking()
                 .Where(l => l.SubjectId == request.SubjectId)
                 .OrderBy(l => l.Order)
                 .Select(l => new LectureResponse(
                     l.Id, l.Title, l.Description, l.Order, l.DurationInSeconds, l.ThumbnailFileId != null,
                     l.MyListItems.Any(m => m.UserId == userId),
                     l.DownloadedLectures.Any(d => d.UserId == userId)))
                 .ToListAsync(cancellationToken);

            return lectures;
        }
    }
}
