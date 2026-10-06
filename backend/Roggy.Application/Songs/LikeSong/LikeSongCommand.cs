namespace Roggy.Application.Songs.LikeSong;

public sealed record LikeSongCommand(
    Guid SongId,
    Guid UserId);