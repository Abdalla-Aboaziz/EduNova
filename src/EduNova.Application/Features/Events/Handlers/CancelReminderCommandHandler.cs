using EduNova.Application.Common.Interfaces;
using EduNova.Application.Common.Results;
using EduNova.Application.Features.Events.Commands;
using EduNova.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EduNova.Application.Features.Events.Handlers;

public sealed class CancelReminderCommandHandler(
    IApplicationDbContext context,
    ICurrentUserService currentUser,
    IConfiguration configuration)
    : IRequestHandler<CancelReminderCommand, Result>
{
    public async Task<Result> Handle(CancelReminderCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUser.UserId
            ?? configuration["EduNova:DemoStudentId"]
            ?? "demo-student-1";

        var reminders = await context.Reminders
            .Where(r => r.EventId == request.EventId && r.UserId == userId)
            .ToListAsync(cancellationToken);

        if (reminders.Count == 0)
        {
            return Result.Failure(Error.NotFound(
                "REMINDER_NOT_FOUND",
                "No reminder was found for this event."));
        }

        context.Reminders.RemoveRange(reminders);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
