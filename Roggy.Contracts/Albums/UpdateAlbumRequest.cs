namespace Roggy.Contracts.Albums;

public sealed record UpdateAlbumRequest(
    string Title,
    string? Description,
    string? CoverImageUrl,
    DateTime? ReleaseDate);