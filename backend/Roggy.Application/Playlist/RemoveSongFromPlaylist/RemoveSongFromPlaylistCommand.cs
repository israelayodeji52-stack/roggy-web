namespace Roggy.Application.Playlist.RemoveSongFromPlaylist;

public sealed record RemoveSongFromPlaylistCommand(
    Guid PlaylistId,
    Guid UserId,
    Guid SongId);