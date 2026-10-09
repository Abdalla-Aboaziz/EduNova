using EduNova.Application.Common.Interfaces;
using EduNova.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Application.Features.Events.Jobs;

public sealed class ReminderDispatchJob(
    IApplicationDbContext context,
    IPushNotificationService pushNotificationService,
    ILogger<ReminderDispatchJob> logger)
{
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var dueReminders = await context.Reminders
            .Include(r => r.Event)
            .Where(r => !r.IsSent && r.RemindAt <= now)
            .ToListAsync(cancellationToken);

        if (dueReminders.Count == 0)
        {
            return;
        }

        foreach (var reminder in dueReminders)
        {
            var title = reminder.Event?.Title ?? "Reminder";
            var body = string.IsNullOrWhiteSpace(reminder.Message)
                ? $"Reminder: {title}"
                : reminder.Message;

            await pushNotificationService.SendToUserAsync(
                reminder.UserId, title, body, cancellationToken);

            reminder.IsSent = true;
            reminder.SentAt = now;
        }

        await context.SaveChangesAsync(cancellationToken);

        logger.LogInformation("Dispatched {Count} due reminders.", dueReminders.Count);
    }
}
