namespace Roggy.Contracts.Remixes;

public sealed record RemixResponse(
    Guid Id,
    Guid OriginalSongId,
    Guid RemixerArtistId,
    string Title,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration,
    string ContentRating,
    bool IsPublished,
    DateTime CreatedAt,
    DateTime UpdatedAt);