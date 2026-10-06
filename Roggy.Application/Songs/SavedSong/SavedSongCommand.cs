namespace Roggy.Application.Songs.SaveSong;

public sealed record SaveSongCommand(
    Guid SongId,
    Guid UserId);