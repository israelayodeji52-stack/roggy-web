using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class LiveStream : BaseEntity
{
    public Guid LiveEventId { get; private set; }

    public string PlaybackUrl { get; private set; } = string.Empty;

    public string StreamStatus { get; private set; } = "Offline";

    public DateTime? StartedAt { get; private set; }

    public DateTime? EndedAt { get; private set; }

    public LiveEvent LiveEvent { get; private set; } = null!;

    private LiveStream()
    {
    }

    public LiveStream(
        Guid liveEventId,
        string playbackUrl)
    {
        LiveEventId = liveEventId;
        PlaybackUrl = playbackUrl;
        StreamStatus = "Offline";
    }

    public void Start()
    {
        StreamStatus = "Live";
        StartedAt = DateTime.UtcNow;
        EndedAt = null;

        UpdateTimestamp();
    }

    public void End()
    {
        StreamStatus = "Ended";
        EndedAt = DateTime.UtcNow;

        UpdateTimestamp();
    }
}