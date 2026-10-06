namespace Roggy.Application.Albums.UpdateAlbum;

public sealed record UpdateAlbumCommand(
    Guid AlbumId,
    string Title,
    string? Description,
    string? CoverImageUrl,
    DateTime? ReleaseDate);