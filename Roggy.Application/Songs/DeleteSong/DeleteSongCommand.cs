namespace Roggy.Application.Songs.DeleteSong;

public sealed record DeleteSongCommand(
    Guid SongId);