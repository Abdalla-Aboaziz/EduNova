using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class GetSubjectLecturesQueryHandler : IRequestHandler<GetSubjectLecturesQuery, List<LectureResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetSubjectLecturesQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<List<LectureResponse>> Handle(GetSubjectLecturesQuery request, CancellationToken cancellationToken)
        {
            var userId = _currentUser.UserId;

            return await _context.Lectures
                 .AsNoTracking()
                 .Where(l => l.SubjectId == request.SubjectId)
                 .OrderBy(l => l.Order)
                 .Select(l => new LectureResponse(
                     l.Id, l.Title, l.Description, l.Order, l.DurationInSeconds, l.ThumbnailFileId != null,
                     l.MyListItems.Any(m => m.UserId == userId),
                     l.DownloadedLectures.Any(d => d.UserId == userId)))
                 .ToListAsync(cancellationToken);

        }
    }
}
