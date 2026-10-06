namespace Roggy.Contracts.Songs;

public sealed record SavedSongResponse(
    Guid Id,
    Guid SongId,
    Guid UserId,
    DateTime CreatedAt);