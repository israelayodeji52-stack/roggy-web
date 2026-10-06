namespace Roggy.Application.Playlist.MakePlaylistPrivate;

public sealed record MakePlaylistPrivateCommand(
    Guid PlaylistId,
    Guid UserId);