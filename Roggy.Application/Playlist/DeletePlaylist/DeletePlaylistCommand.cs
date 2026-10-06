namespace Roggy.Application.Playlist.DeletePlaylist;

public sealed record DeletePlaylistCommand(
    Guid PlaylistId,
    Guid UserId);