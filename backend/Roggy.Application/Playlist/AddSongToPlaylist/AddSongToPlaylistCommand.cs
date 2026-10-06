namespace Roggy.Application.Playlist.AddSongToPlaylist;

public sealed record AddSongToPlaylistCommand(
    Guid PlaylistId,
    Guid UserId,
    Guid SongId);