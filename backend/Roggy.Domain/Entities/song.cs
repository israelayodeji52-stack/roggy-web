using Roggy.Domain.Common;
using Roggy.Domain.Enums;

namespace Roggy.Domain.Entities;

public class Song : BaseEntity
{
    public Guid ArtistId { get; private set; }
    public Guid? AlbumId { get; private set; }
    public Guid GenreId { get; private set; }
    public string Title { get; private set; } = string.Empty;
    public string? Description { get; private set; }
    public string AudioUrl { get; private set; } = string.Empty;
    public string? CoverImageUrl { get; private set; }
    public TimeSpan Duration { get; private set; }
    public DateTime? ReleaseDate { get; private set; }
    public ContentRating ContentRating { get; private set; }
    public bool AllowRemixing { get; private set; }
    public bool IsPublished { get; private set; }
    public long PlayCount { get; private set; }
    public int TrackNumber { get; private set; }

    public ArtistProfile Artist { get; private set; } = null!;
    public Album? Album { get; private set; }
    public Genre Genre { get; private set; } = null!;
    public ICollection<Remix> Remixes { get; private set; } = new List<Remix>();

    private Song()
    {
    }

    public Song(
        Guid artistId,
        Guid genreId,
        string title,
        string audioUrl,
        TimeSpan duration)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Song title is required.",
                nameof(title));

        if (string.IsNullOrWhiteSpace(audioUrl))
            throw new ArgumentException(
                "Audio URL is required.",
                nameof(audioUrl));

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException(
                "Song duration must be greater than zero.",
                nameof(duration));

        ArtistId = artistId;
        GenreId = genreId;
        Title = title.Trim();
        AudioUrl = audioUrl.Trim();
        Duration = duration;

        ContentRating = ContentRating.Clean;
        AllowRemixing = false;
        IsPublished = false;
        PlayCount = 0;
        TrackNumber = 1;
    }

    public void SetAlbum(Guid? albumId)
    {
        AlbumId = albumId;
    }

    public void UpdateSong(
        Guid genreId,
        string title,
        string? description,
        string audioUrl,
        string? coverImageUrl,
        TimeSpan duration,
        DateTime? releaseDate,
        ContentRating contentRating,
        bool allowRemixing,
        int trackNumber)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException(
                "Song title is required.",
                nameof(title));

        if (string.IsNullOrWhiteSpace(audioUrl))
            throw new ArgumentException(
                "Audio URL is required.",
                nameof(audioUrl));

        if (duration <= TimeSpan.Zero)
            throw new ArgumentException(
                "Song duration must be greater than zero.",
                nameof(duration));

        if (trackNumber <= 0)
            throw new ArgumentException(
                "Track number must be greater than zero.",
                nameof(trackNumber));

        GenreId = genreId;
        Title = title.Trim();
        Description = description?.Trim();
        AudioUrl = audioUrl.Trim();
        CoverImageUrl = coverImageUrl?.Trim();
        Duration = duration;
        ReleaseDate = releaseDate;
        ContentRating = contentRating;
        AllowRemixing = allowRemixing;
        TrackNumber = trackNumber;
    }

    public void Publish()
    {
        IsPublished = true;
    }

    public void Unpublish()
    {
        IsPublished = false;
    }

    public void IncrementPlayCount()
    {
        PlayCount++;
    }
}