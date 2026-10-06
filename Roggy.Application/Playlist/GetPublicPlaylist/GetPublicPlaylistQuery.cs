namespace Roggy.Application.Playlist.GetPublicPlaylists;

public sealed record GetPublicPlaylistsQuery(
    int Limit = 20);