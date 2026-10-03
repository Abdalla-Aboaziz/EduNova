using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public class GetMyListQueryHandler : IRequestHandler<GetMyListQuery, List<MyListResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public GetMyListQueryHandler(IApplicationDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }
        public async Task<List<MyListResponse>> Handle(GetMyListQuery request, CancellationToken cancellationToken)
        {
            var userId = "101"; // TODO: Implement user authentication and get the current user ID

            return await _context.MyListItems
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                 .OrderByDescending(m => m.AddedAt)
            .Join(_context.Lectures, m => m.LectureId, l => l.Id,
                (m, l) => new MyListResponse(l.Id, l.Title, l.DurationInSeconds, m.AddedAt))
            .ToListAsync(cancellationToken);
        }
    }
}
