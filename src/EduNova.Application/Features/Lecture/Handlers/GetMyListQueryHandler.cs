using EduNova.Application.Common.Interfaces;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetMyListQueryHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
        : IRequestHandler<GetMyListQuery, List<MyListResponse>>
    {
        public async Task<List<MyListResponse>> Handle(GetMyListQuery request, CancellationToken cancellationToken)
        {
            var userId = "101"; // TODO: Implement user authentication and get the current user ID

            var items = await context.MyListItems
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                 .OrderByDescending(m => m.AddedAt)
            .Join(context.Lectures, m => m.LectureId, l => l.Id,
                (m, l) => new MyListResponse(l.Id, l.Title, l.DurationInSeconds, m.AddedAt))
            .ToListAsync(cancellationToken);

            return items;
        }
    }
}
