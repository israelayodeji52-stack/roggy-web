using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class Playlist : BaseEntity
{
    public Guid UserId { get; private set; }

    public string Name { get; private set; } = string.Empty;

    public string? Description { get; private set; }

    public string? CoverImageUrl { get; private set; }

    public bool IsPublic { get; private set; }

    public User User { get; private set; } = null!;

    public ICollection<PlaylistItem> Items { get; private set; }
        = new List<PlaylistItem>();

    private Playlist()
    {
    }

    public Playlist(
        Guid userId,
        string name)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(
                "User ID is required.",
                nameof(userId));

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Playlist name is required.",
                nameof(name));

        UserId = userId;
        Name = name.Trim();
        IsPublic = false;
    }

    public void Update(
        string name,
        string? description,
        string? coverImageUrl)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException(
                "Playlist name is required.",
                nameof(name));

        Name = name.Trim();
        Description = description?.Trim();
        CoverImageUrl = coverImageUrl?.Trim();

        UpdateTimestamp();
    }

    public void MakePublic()
    {
        IsPublic = true;
        UpdateTimestamp();
    }

    public void MakePrivate()
    {
        IsPublic = false;
        UpdateTimestamp();
    }
}