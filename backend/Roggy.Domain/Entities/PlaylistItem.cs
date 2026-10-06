using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class PlaylistItem : BaseEntity
{
    public Guid PlaylistId { get; private set; }

    public Guid SongId { get; private set; }

    public int Position { get; private set; }

    public DateTime AddedAt { get; private set; }

    public Playlist Playlist { get; private set; } = null!;

    public Song Song { get; private set; } = null!;

    private PlaylistItem()
    {
    }

    public PlaylistItem(
        Guid playlistId,
        Guid songId,
        int position)
    {
        if (playlistId == Guid.Empty)
            throw new ArgumentException(
                "Playlist ID is required.",
                nameof(playlistId));

        if (songId == Guid.Empty)
            throw new ArgumentException(
                "Song ID is required.",
                nameof(songId));

        if (position <= 0)
            throw new ArgumentException(
                "Playlist position must be greater than zero.",
                nameof(position));

        PlaylistId = playlistId;
        SongId = songId;
        Position = position;
        AddedAt = DateTime.UtcNow;
    }

    public void ChangePosition(int position)
    {
        if (position <= 0)
            throw new ArgumentException(
                "Playlist position must be greater than zero.",
                nameof(position));

        Position = position;
        UpdateTimestamp();
    }
}