namespace Roggy.Application.Songs.UpdateSong;

public sealed record UpdateSongCommand(
    Guid SongId,
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