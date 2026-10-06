namespace Roggy.Application.Playlist.MakePlaylistPublic;

public sealed record MakePlaylistPublicCommand(
    Guid PlaylistId,
    Guid UserId);