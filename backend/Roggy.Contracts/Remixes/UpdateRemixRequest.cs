namespace Roggy.Contracts.Remixes;

public sealed record UpdateRemixRequest(
    string Title,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration,
    int ContentRating);