namespace Roggy.Application.Songs.CreateSong;

public sealed record CreateSongCommand(
    Guid ArtistId,
    Guid? AlbumId,
    Guid GenreId,
    string Title,
    string? Description,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration,
    DateTime? ReleaseDate,
    Roggy.Domain.Enums.ContentRating ContentRating,
    bool AllowRemixing,
    int TrackNumber);