using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class DownloadHistory : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid SongId { get; private set; }

    public DateTime DownloadedAt { get; private set; }

    public User User { get; private set; } = null!;

    public Song Song { get; private set; } = null!;

    private DownloadHistory()
    {
    }

    public DownloadHistory(
        Guid userId,
        Guid songId)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));
        }

        if (songId == Guid.Empty)
        {
            throw new ArgumentException(
                "Song ID is required.",
                nameof(songId));
        }

        UserId = userId;
        SongId = songId;
        DownloadedAt = DateTime.UtcNow;
    }
}