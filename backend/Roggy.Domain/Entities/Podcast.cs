using Roggy.Domain.Common;
using Roggy.Domain.Enums;

namespace Roggy.Domain.Entities;

public class Podcast : BaseEntity
{
    public Guid CreatorId { get; private set; }

    public string Title { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? CoverImageUrl { get; private set; }

    public string Category { get; private set; } = string.Empty;

    public ContentRating ContentRating { get; private set; }

    public bool IsPublished { get; private set; }

    public User Creator { get; private set; } = null!;

    public ICollection<PodcastEpisode> Episodes { get; private set; }
        = new List<PodcastEpisode>();

    private Podcast()
    {
    }

    public Podcast(
        Guid creatorId,
        string title,
        string category)
    {
        CreatorId = creatorId;
        Title = title;
        Category = category;

        ContentRating = ContentRating.Clean;
        IsPublished = false;
    }
}