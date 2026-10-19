using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Pagination;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Lecture.Queries;
using EduNova.Application.Features.Lecture.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Lecture.Handlers
{
    public sealed class GetMyListQueryHandler(
        IApplicationDbContext context, ICurrentUserService currentUser)
        : IRequestHandler<GetMyListQuery, Result<PagedResult<MyListResponse>>>
    {
        public async Task<Result<PagedResult<MyListResponse>>> Handle(GetMyListQuery request, CancellationToken cancellationToken)
        {
            var userId = currentUser.UserId;

            var items = await context.MyListItems
                .AsNoTracking()
                .Where(m => m.UserId == userId)
                 .OrderByDescending(m => m.AddedAt)
            .Join(context.Lectures, m => m.LectureId, l => l.Id,
                (m, l) => new MyListResponse(l.Id, l.Title, l.DurationInSeconds, m.AddedAt))
            .ToPagedResultAsync(request, cancellationToken);
            return Result.Success(items);
        }
    }
}
