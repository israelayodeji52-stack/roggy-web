namespace Roggy.Application.Playlist.ReorderPlaylistSong;

public sealed record ReorderPlaylistSongCommand(
    Guid PlaylistId,
    Guid UserId,
    Guid SongId,
    int NewPosition);