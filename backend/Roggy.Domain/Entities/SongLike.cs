using Roggy.Domain.Common;

namespace Roggy.Domain.Entities;

public class SongLike : BaseEntity
{
    public Guid UserId { get; private set; }

    public Guid SongId { get; private set; }

    public User User { get; private set; } = null!;

    public Song Song { get; private set; } = null!;

    private SongLike()
    {
    }

    public SongLike(
        Guid userId,
        Guid songId)
    {
        UserId = userId;
        SongId = songId;
    }
}