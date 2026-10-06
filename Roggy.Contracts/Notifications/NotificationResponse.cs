namespace Roggy.Contracts.Notifications;

public sealed record NotificationResponse(
    Guid Id,
    Guid UserId,
    string Title,
    string Message,
    string? ActionUrl,
    bool IsRead,
    DateTime? ReadAt,
    DateTime CreatedAt,
    DateTime UpdatedAt);