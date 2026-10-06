namespace Roggy.Contracts.Albums;

public sealed record AlbumSongResponse(
    Guid Id,
    string Title,
    string? CoverImageUrl,
    TimeSpan Duration,
    DateTime? ReleaseDate,
    int TrackNumber,
    bool IsPublished);

public sealed record AlbumResponse(
    Guid Id,
    Guid ArtistId,
    string Title,
    string? Description,
    string? CoverImageUrl,
    DateTime? ReleaseDate,
    bool IsPublished,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<AlbumSongResponse> Songs);