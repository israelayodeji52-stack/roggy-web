namespace Roggy.Application.Remixes.PublishRemix;

public sealed record PublishRemixCommand(
    Guid RemixId,
    Guid UserId);