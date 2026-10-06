using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class LiveChatMessage : BaseEntity
{
    public Guid LiveEventId { get; private set; }

    public Guid UserId { get; private set; }

    public string Message { get; private set; } = string.Empty;

    public bool IsDeleted { get; private set; }

    public LiveEvent LiveEvent { get; private set; } = null!;

    public User User { get; private set; } = null!;

    private LiveChatMessage()
    {
    }

    public LiveChatMessage(
        Guid liveEventId,
        Guid userId,
        string message)
    {
        LiveEventId = liveEventId;
        UserId = userId;
        Message = message;

        IsDeleted = false;
    }

    public void Delete()
    {
        IsDeleted = true;
        UpdateTimestamp();
    }
}