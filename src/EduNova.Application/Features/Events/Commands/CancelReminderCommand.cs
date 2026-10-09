using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Events.Commands;

public class CancelReminderCommand(Guid eventId) : IRequest<Result>
{
    public Guid EventId { get; } = eventId;
}
