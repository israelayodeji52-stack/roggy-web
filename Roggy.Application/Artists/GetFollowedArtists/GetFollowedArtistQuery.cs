namespace Roggy.Application.Artists.GetFollowedArtists;

public sealed record GetFollowedArtistsQuery(
    Guid UserId,
    int Limit = 50);