using EduNova.Application.Common.Results;
using MediatR;

namespace EduNova.Application.Features.Events.Commands;

public class SetReminderCommand : IRequest<Result<Guid>>
{
    public Guid EventId { get; set; }
    public DateTime RemindAt { get; set; }
    public string? Message { get; set; }
}
