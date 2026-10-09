using EduNova.Application.Common.Results;
using EduNova.Application.Features.Events.Responses;
using MediatR;

namespace EduNova.Application.Features.Events.Queries;

public class GetEventsQuery(EventsRange range) : IRequest<Result<List<EventResponse>>>
{
    public EventsRange Range { get; } = range;
}
