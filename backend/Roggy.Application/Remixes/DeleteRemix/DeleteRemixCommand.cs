namespace Roggy.Application.Remixes.DeleteRemix;

public sealed record DeleteRemixCommand(
    Guid RemixId,
    Guid UserId);