namespace Roggy.Application.Remixes.UnpublishRemix;

public sealed record UnpublishRemixCommand(
    Guid RemixId,
    Guid UserId);