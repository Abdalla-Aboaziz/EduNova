using EduNova.Application.Common.Results;
using EduNova.Application.Features.Notices.Queries;
using EduNova.Application.Features.Notices.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Notices.Handlers;

public sealed class GetNoticesQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetNoticesQuery, Result<List<NoticeResponse>>>
{
    public async Task<Result<List<NoticeResponse>>> Handle(
        GetNoticesQuery request, CancellationToken cancellationToken)
    {
        var notices = await context.Notices
            .AsNoTracking()
            .Where(n => n.IsActive)
            .OrderByDescending(n => n.PublishedAt)
            .Select(n => new NoticeResponse
            {
                Id = n.Id,
                Title = n.Title,
                Content = n.Content,
                PublishedAt = n.PublishedAt
            })
            .ToListAsync(cancellationToken);

        return Result.Success(notices);
    }
}
