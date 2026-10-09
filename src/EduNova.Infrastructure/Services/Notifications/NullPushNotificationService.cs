using EduNova.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace EduNova.Infrastructure.Services.Notifications;

public sealed class NullPushNotificationService(
    ILogger<NullPushNotificationService> logger) : IPushNotificationService
{
    public Task SendToUserAsync(
        string userId, string title, string body, CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Push notifications are disabled (Firebase credentials not configured). Skipped message for user {UserId}.",
            userId);

        return Task.CompletedTask;
    }

    public Task SendToTokenAsync(
        string token, string title, string body, CancellationToken cancellationToken = default)
    {
        logger.LogWarning(
            "Push notifications are disabled (Firebase credentials not configured). Skipped message for token {Token}.",
            token);

        return Task.CompletedTask;
    }
}
