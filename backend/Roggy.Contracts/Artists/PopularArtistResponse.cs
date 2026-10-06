namespace Roggy.Contracts.Artists;

public sealed record PopularArtistResponse(
    Guid Id,
    Guid UserId,
    string StageName,
    string? Biography,
    string? ProfileImageUrl,
    string? BannerImageUrl,
    bool IsVerified,
    DateTime CreatedAt,
    DateTime UpdatedAt);