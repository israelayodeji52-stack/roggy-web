using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class ContentPreference : BaseEntity
{
    public Guid UserId { get; private set; }

    public bool AllowExplicitMusic { get; private set; }

    public bool AllowExplicitPodcasts { get; private set; }

    public bool AllowMatureContent { get; private set; }

    public User User { get; private set; } = null!;

    private ContentPreference()
    {
    }

    public ContentPreference(Guid userId)
    {
        UserId = userId;

        AllowExplicitMusic = false;
        AllowExplicitPodcasts = false;
        AllowMatureContent = false;
    }

    public void Update(
        bool allowExplicitMusic,
        bool allowExplicitPodcasts,
        bool allowMatureContent)
    {
        AllowExplicitMusic = allowExplicitMusic;
        AllowExplicitPodcasts = allowExplicitPodcasts;
        AllowMatureContent = allowMatureContent;

        UpdateTimestamp();
    }
}