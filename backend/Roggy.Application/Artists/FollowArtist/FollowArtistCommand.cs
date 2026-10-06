namespace Roggy.Application.Artists.FollowArtist;

public sealed record FollowArtistCommand(
    Guid UserId,
    Guid ArtistId);