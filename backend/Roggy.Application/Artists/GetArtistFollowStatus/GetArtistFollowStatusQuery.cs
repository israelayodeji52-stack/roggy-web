namespace Roggy.Application.Artists.GetArtistFollowStatus;

public sealed record GetArtistFollowStatusQuery(
    Guid UserId,
    Guid ArtistId);