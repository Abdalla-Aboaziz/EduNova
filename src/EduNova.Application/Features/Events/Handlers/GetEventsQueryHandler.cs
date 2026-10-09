using EduNova.Application.Common.Results;
using EduNova.Application.Features.Events.Queries;
using EduNova.Application.Features.Events.Responses;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EduNova.Application.Features.Events.Handlers;

public sealed class GetEventsQueryHandler(IApplicationDbContext context)
    : IRequestHandler<GetEventsQuery, Result<List<EventResponse>>>
{
    public async Task<Result<List<EventResponse>>> Handle(
        GetEventsQuery request, CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var today = now.Date;

        DateTime rangeStart;
        DateTime rangeEnd;

        if (request.Range == EventsRange.Today)
        {
            rangeStart = today;
            rangeEnd = today.AddDays(1);
        }
        else
        {
            var daysSinceWeekStart = (7 + (today.DayOfWeek - DayOfWeek.Saturday)) % 7;
            rangeStart = today.AddDays(-daysSinceWeekStart);
            rangeEnd = rangeStart.AddDays(7);
        }

        var events = await context.Events
            .AsNoTracking()
            .Where(e => e.StartAt >= rangeStart && e.StartAt < rangeEnd)
            .OrderBy(e => e.StartAt)
            .Select(e => new EventResponse
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Type = e.Type,
                StartAt = e.StartAt,
                EndAt = e.EndAt,
                Location = e.Location,
                SubjectId = e.SubjectId,
                SubjectName = e.Subject != null ? e.Subject.Name : null
            })
            .ToListAsync(cancellationToken);

        return Result.Success(events);
    }
}
