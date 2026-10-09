namespace EduNova.Application.Common.Interfaces;

public interface IPushNotificationService
{
    Task SendToUserAsync(string userId, string title, string body, CancellationToken cancellationToken = default);

    Task SendToTokenAsync(string token, string title, string body, CancellationToken cancellationToken = default);
}
