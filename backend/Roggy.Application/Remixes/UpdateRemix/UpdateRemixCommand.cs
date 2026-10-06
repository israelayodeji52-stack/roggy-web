using Roggy.Domain.Enums;

namespace Roggy.Application.Remixes.UpdateRemix;

public sealed record UpdateRemixCommand(
    Guid RemixId,
    Guid UserId,
    string Title,
    string AudioUrl,
    string? CoverImageUrl,
    TimeSpan Duration,
    ContentRating ContentRating);