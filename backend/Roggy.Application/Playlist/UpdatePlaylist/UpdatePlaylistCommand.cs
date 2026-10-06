namespace Roggy.Application.Playlist.UpdatePlaylist;

public sealed record UpdatePlaylistCommand(
    Guid PlaylistId,
    Guid UserId,
    string Name,
    string? Description,
    string? CoverImageUrl);