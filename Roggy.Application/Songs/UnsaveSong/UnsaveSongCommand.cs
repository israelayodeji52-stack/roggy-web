namespace Roggy.Application.Songs.UnsaveSong;

public sealed record UnsaveSongCommand(
    Guid SongId,
    Guid UserId);