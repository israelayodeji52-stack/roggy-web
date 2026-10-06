namespace Roggy.Application.Remixes.CreateRemix;

public sealed record CreateRemixCommand(
    Guid UserId,
    Guid OriginalSongId,
    string Title,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration);