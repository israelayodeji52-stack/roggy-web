namespace Roggy.Application.Playlist.CreatePlaylist;

public sealed record CreatePlaylistCommand(
    Guid UserId,
    string Name);