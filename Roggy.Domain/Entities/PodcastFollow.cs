using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class PodcastFollow : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid PodcastId { get; private set; }

    public User User { get; private set; } = null!;

    public Podcast Podcast { get; private set; } = null!;

    private PodcastFollow()
    {
    }

    public PodcastFollow(
        Guid userId,
        Guid podcastId)
    {
        UserId = userId;
        PodcastId = podcastId;
    }
}