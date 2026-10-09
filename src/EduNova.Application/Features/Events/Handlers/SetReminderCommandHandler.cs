using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Events.Commands;
using EduNova.Application.Interfaces;
using EduNova.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Events.Handlers;

public sealed class SetReminderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IConfiguration configuration)
    : IRequestHandler<SetReminderCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(SetReminderCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? configuration["EduNova:DemoStudentId"]
            ?? "demo-student-1";

        var eventExists = await context.Events
            .AnyAsync(e => e.Id == request.EventId, cancellationToken);

        if (!eventExists)
        {
            return Result.Failure<Guid>(Error.NotFound("Event", request.EventId));
        }

        var reminder = await context.Reminders
            .FirstOrDefaultAsync(
                r => r.EventId == request.EventId && r.UserId == userId && !r.IsSent,
                cancellationToken);

        if (reminder is null)
        {
            reminder = new Reminder
            {
                EventId = request.EventId,
                UserId = userId,
                RemindAt = request.RemindAt,
                Message = request.Message
            };

            context.Reminders.Add(reminder);
        }
        else
        {
            reminder.RemindAt = request.RemindAt;
            reminder.Message = request.Message;
            reminder.UpdatedAt = DateTime.UtcNow;
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Success(reminder.Id);
    }
}
