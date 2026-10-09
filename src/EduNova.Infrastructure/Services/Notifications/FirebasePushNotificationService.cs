using EduNova.Application.Common.Interfaces;
using EduNova.Application.Interfaces;
using FirebaseAdmin.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EduNova.Infrastructure.Services.Notifications;

public sealed class FirebasePushNotificationService(
    IApplicationDbContext context,
    ILogger<FirebasePushNotificationService> logger) : IPushNotificationService
{
    public async Task SendToUserAsync(
        string userId, string title, string body, CancellationToken cancellationToken = default)
    {
        var tokens = await context.DeviceTokens
            .AsNoTracking()
            .Where(t => t.UserId == userId && t.IsActive)
            .Select(t => t.Token)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            await SendToTokenAsync(token, title, body, cancellationToken);
        }
    }

    public async Task SendToTokenAsync(
        string token, string title, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var message = new Message
            {
                Fid = token,
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                }
            };

            await FirebaseMessaging.DefaultInstance.SendAsync(message, cancellationToken);
        }
        catch (FirebaseMessagingException ex)
        {
            logger.LogError(ex, "Failed to send push notification to token {Token}.", token);
        }
    }
}
