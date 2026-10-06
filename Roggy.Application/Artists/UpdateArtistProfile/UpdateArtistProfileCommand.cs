namespace Roggy.Application.Artists.UpdateArtistProfile;

public sealed record UpdateArtistProfileCommand(
    Guid ArtistProfileId,
    string StageName,
    string? Biography,
    string? ProfileImageUrl,
    string? BannerImageUrl);