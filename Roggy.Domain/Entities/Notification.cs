using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class Notification : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public string? ActionUrl { get; private set; }

    public bool IsRead { get; private set; }

    public DateTime? ReadAt { get; private set; }

    public User User { get; private set; } = null!;

    private Notification()
    {
    }

    public Notification(
        Guid userId,
        string title,
        string message,
        string? actionUrl = null)
    {
        UserId = userId;
        Title = title;
        Message = message;
        ActionUrl = actionUrl;

        IsRead = false;
    }

    public void MarkAsRead()
    {
        if (IsRead)
        {
            return;
        }

        IsRead = true;
        ReadAt = DateTime.UtcNow;

        UpdateTimestamp();
    }
}