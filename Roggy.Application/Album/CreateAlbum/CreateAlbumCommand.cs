namespace Roggy.Application.Albums.CreateAlbum;

public sealed record CreateAlbumCommand(
    Guid ArtistId,
    string Title);