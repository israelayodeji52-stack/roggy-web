using Roggy.Domain.Common;
using Roggy.Domain.Enums;

namespace Roggy.Domain.Entities;

public class LiveEvent : BaseEntity
{
    public Guid ArtistId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? CoverImageUrl { get; private set; }

    public DateTime ScheduledStart { get; private set; }

    public DateTime ScheduledEnd { get; private set; }

    public LiveEventStatus Status { get; private set; }

    public bool IsPublic { get; private set; }

    public ContentRating ContentRating { get; private set; }

    public ArtistProfile Artist { get; private set; } = null!;

    public LiveStream? LiveStream { get; private set; }

    public ICollection<LiveChatMessage> ChatMessages { get; private set; }
        = new List<LiveChatMessage>();

    private LiveEvent()
    {
    }

    public LiveEvent(
        Guid artistId,
        string title,
        DateTime scheduledStart,
        DateTime scheduledEnd)
    {
        ArtistId = artistId;
        Title = title;
        ScheduledStart = scheduledStart;
        ScheduledEnd = scheduledEnd;

        Status = LiveEventStatus.Scheduled;
        IsPublic = true;
        ContentRating = ContentRating.Clean;
    }

    public void Start()
    {
        Status = LiveEventStatus.Live;
        UpdateTimestamp();
    }

    public void End()
    {
        Status = LiveEventStatus.Ended;
        UpdateTimestamp();
    }

    public void Cancel()
    {
        Status = LiveEventStatus.Cancelled;
        UpdateTimestamp();
    }
}