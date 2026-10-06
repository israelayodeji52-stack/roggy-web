namespace Roggy.Application.Notifications.MarkNotificationAsRead;

public sealed record MarkNotificationAsReadCommand(
    Guid UserId,
    Guid NotificationId);