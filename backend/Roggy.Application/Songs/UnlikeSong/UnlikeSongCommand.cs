namespace Roggy.Application.Songs.UnlikeSong;

public sealed record UnlikeSongCommand(
    Guid SongId,
    Guid UserId);