namespace Roggy.Contracts.Songs;

public sealed record SongPlayResponse(
    Guid Id,
    string Title,
    long PlayCount);