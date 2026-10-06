namespace Roggy.Contracts.Songs;

public sealed record SongLikeResponse(
    Guid Id,
    Guid SongId,
    Guid UserId,
    DateTime CreatedAt);