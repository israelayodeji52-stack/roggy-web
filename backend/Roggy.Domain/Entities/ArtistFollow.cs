using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class ArtistFollow : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid ArtistId { get; private set; }

    public User User { get; private set; } = null!;

    public ArtistProfile Artist { get; private set; } = null!;

    private ArtistFollow()
    {
    }

    public ArtistFollow(
        Guid userId,
        Guid artistId)
    {
        UserId = userId;
        ArtistId = artistId;
    }
}