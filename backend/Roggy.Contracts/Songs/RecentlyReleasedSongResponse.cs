namespace Roggy.Contracts.Songs;

public sealed record RecentlyReleasedSongResponse(
    Guid Id,
    Guid ArtistId,
    Guid? AlbumId,
    Guid GenreId,
    string Title,
    string? Description,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration,
    DateTime? ReleaseDate,
    int ContentRating,
    bool AllowRemixing,
    bool IsPublished,
    long PlayCount,
    int TrackNumber,
    DateTime CreatedAt,
    DateTime UpdatedAt);