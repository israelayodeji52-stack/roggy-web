using Roggy.Domain.Common;
using Roggy.Domain.Enums;

namespace Roggy.Domain.Entities;

public class PodcastEpisode : BaseEntity
{
    public Guid PodcastId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string AudioUrl { get; private set; } = string.Empty;

    public TimeSpan Duration { get; private set; }

    public int EpisodeNumber { get; private set; }

    public int SeasonNumber { get; private set; }

    public DateTime? PublishedAt { get; private set; }

    public ContentRating ContentRating { get; private set; }

    public bool IsPublished { get; private set; }

    public long PlayCount { get; private set; }

    public Podcast Podcast { get; private set; } = null!;

    private PodcastEpisode()
    {
    }

    public PodcastEpisode(
        Guid podcastId,
        string title,
        string audioUrl,
        TimeSpan duration,
        int episodeNumber,
        int seasonNumber)
    {
        PodcastId = podcastId;
        Title = title;
        AudioUrl = audioUrl;
        Duration = duration;
        EpisodeNumber = episodeNumber;
        SeasonNumber = seasonNumber;

        ContentRating = ContentRating.Clean;
        IsPublished = false;
        PlayCount = 0;
    }
}