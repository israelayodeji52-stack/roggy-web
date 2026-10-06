namespace Roggy.Contracts.Playlists;

public sealed record ReorderPlaylistSongRequest(
    int NewPosition);