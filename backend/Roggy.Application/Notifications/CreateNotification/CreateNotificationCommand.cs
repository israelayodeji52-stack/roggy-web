namespace Roggy.Application.Notifications.CreateNotification;

public sealed record CreateNotificationCommand(
    Guid UserId,
    string Title,
    string Message,
    string? ActionUrl = null);