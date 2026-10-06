using Roggy.Domain.Common;
using Roggy.Domain.Enums;

namespace Roggy.Domain.Entities;

public class Remix : BaseEntity
{
    public Guid OriginalSongId { get; private set; }

    public Guid RemixerArtistId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string AudioUrl { get; private set; } = string.Empty;

    public string? CoverImageUrl { get; private set; }

    public TimeSpan Duration { get; private set; }

    public ContentRating ContentRating { get; private set; }

    public bool IsPublished { get; private set; }

    public Song OriginalSong { get; private set; } = null!;

    public ArtistProfile RemixerArtist { get; private set; } = null!;

    private Remix()
    {
    }

    public Remix(
        Guid originalSongId,
        Guid remixerArtistId,
        string title,
        string audioUrl,
        TimeSpan duration)
    {
        if (originalSongId == Guid.Empty)
        {
            throw new ArgumentException(
                "Original song ID is required.",
                nameof(originalSongId));
        }

        if (remixerArtistId == Guid.Empty)
        {
            throw new ArgumentException(
                "Remixer artist ID is required.",
                nameof(remixerArtistId));
        }

        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Remix title is required.",
                nameof(title));
        }

        if (string.IsNullOrWhiteSpace(audioUrl))
        {
            throw new ArgumentException(
                "Remix audio URL is required.",
                nameof(audioUrl));
        }

        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException(
                "Remix duration must be greater than zero.",
                nameof(duration));
        }

        OriginalSongId = originalSongId;
        RemixerArtistId = remixerArtistId;
        Title = title.Trim();
        AudioUrl = audioUrl.Trim();
        Duration = duration;

        ContentRating = ContentRating.Clean;
        IsPublished = false;
    }

    public void Update(
        string title,
        string audioUrl,
        string? coverImageUrl,
        TimeSpan duration,
        ContentRating contentRating)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new ArgumentException(
                "Remix title is required.",
                nameof(title));
        }

        if (string.IsNullOrWhiteSpace(audioUrl))
        {
            throw new ArgumentException(
                "Remix audio URL is required.",
                nameof(audioUrl));
        }

        if (duration <= TimeSpan.Zero)
        {
            throw new ArgumentException(
                "Remix duration must be greater than zero.",
                nameof(duration));
        }

        Title = title.Trim();
        AudioUrl = audioUrl.Trim();
        CoverImageUrl = coverImageUrl?.Trim();
        Duration = duration;
        ContentRating = contentRating;

        UpdateTimestamp();
    }

    public void Publish()
    {
        IsPublished = true;
        UpdateTimestamp();
    }

    public void Unpublish()
    {
        IsPublished = false;
        UpdateTimestamp();
    }
}