namespace Roggy.Application.Artists.CreateArtistProfile;

public sealed record CreateArtistProfileCommand(
    Guid UserId,
    string StageName);