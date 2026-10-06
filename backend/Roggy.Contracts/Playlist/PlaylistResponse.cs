namespace Roggy.Contracts.Playlists;

public sealed record PlaylistSongResponse(
    Guid Id,
    Guid ArtistId,
    Guid? AlbumId,
    Guid GenreId,
    string Title,
    string? CoverImageUrl,
    TimeSpan Duration,
    DateTime? ReleaseDate,
    int TrackNumber,
    long PlayCount);

public sealed record PlaylistResponse(
    Guid Id,
    Guid UserId,
    string Name,
    string? Description,
    string? CoverImageUrl,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    IReadOnlyList<PlaylistSongResponse> Songs);