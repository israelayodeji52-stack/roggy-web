namespace Roggy.Contracts.Remixes;

public sealed record CreateRemixRequest(
    Guid OriginalSongId,
    string Title,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration);