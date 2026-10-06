namespace Roggy.Application.Artists.UnfollowArtist;

public sealed record UnfollowArtistCommand(
    Guid UserId,
    Guid ArtistId);