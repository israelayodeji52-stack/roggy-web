namespace Roggy.Contracts.Songs;

public sealed record UpdateSongRequest(
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
    int TrackNumber);