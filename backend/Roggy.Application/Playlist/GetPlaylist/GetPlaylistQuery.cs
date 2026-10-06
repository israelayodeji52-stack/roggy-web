namespace Roggy.Application.Playlist.GetPlaylist;

public sealed record GetPlaylistQuery(
    Guid PlaylistId,
    Guid UserId);