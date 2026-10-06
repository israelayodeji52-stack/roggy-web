using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class ListeningHistory : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid SongId { get; private set; }

    public DateTime StartedAt { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public TimeSpan DurationPlayed { get; private set; }

    public User User { get; private set; } = null!;

    public Song Song { get; private set; } = null!;

    private ListeningHistory()
    {
    }

    public ListeningHistory(
        Guid userId,
        Guid songId,
        DateTime startedAt)
    {
        UserId = userId;
        SongId = songId;
        StartedAt = startedAt;

        DurationPlayed = TimeSpan.Zero;
    }

    public void Complete(
        DateTime completedAt,
        TimeSpan durationPlayed)
    {
        CompletedAt = completedAt;
        DurationPlayed = durationPlayed;
    }
}