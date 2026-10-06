namespace Roggy.Contracts.Songs;

public sealed record SongActionResponse(
    Guid Id,
    string Title,
    bool IsPublished,
    DateTime UpdatedAt);