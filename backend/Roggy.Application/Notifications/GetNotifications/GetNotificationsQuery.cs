namespace Roggy.Application.Notifications.GetNotifications;

public sealed record GetNotificationsQuery(
    Guid UserId,
    int Limit = 50);