namespace Roggy.Application.Remixes.GetSongRemixes;

public sealed record GetSongRemixesQuery(
    Guid OriginalSongId,
    int Limit = 50);