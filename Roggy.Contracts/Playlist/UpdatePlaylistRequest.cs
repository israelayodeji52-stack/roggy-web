namespace Roggy.Contracts.Playlists;

public sealed record UpdatePlaylistRequest(
    string Name,
    string? Description,
    string? CoverImageUrl);