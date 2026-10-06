namespace Roggy.Contracts.Artists;

public sealed record UpdateArtistProfileRequest(
    string StageName,
    string? Biography,
    string? ProfileImageUrl,
    string? BannerImageUrl);