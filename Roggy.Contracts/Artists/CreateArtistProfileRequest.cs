namespace Roggy.Contracts.Artists;

public sealed record CreateArtistProfileRequest(
    Guid UserId,
    string StageName);