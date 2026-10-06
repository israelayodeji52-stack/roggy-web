namespace Roggy.Application.Songs.UnpublishSong;

public sealed record UnpublishSongCommand(
    Guid SongId);