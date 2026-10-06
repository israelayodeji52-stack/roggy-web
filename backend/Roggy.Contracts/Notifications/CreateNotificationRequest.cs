namespace Roggy.Contracts.Notifications;

public sealed record CreateNotificationRequest(
    string Title,
    string Message,
    string? ActionUrl = null);