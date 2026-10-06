namespace Roggy.Contracts.Albums;

public sealed record CreateAlbumRequest(
    Guid ArtistId,
    string Title);