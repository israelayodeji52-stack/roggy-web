namespace Roggy.Contracts.Songs;

public sealed record SavedSongStatusResponse(
    Guid SongId,
    bool IsSaved);