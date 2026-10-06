namespace Roggy.Application.Downloads.DownloadSong;

public sealed record DownloadSongCommand(
    Guid UserId,
    Guid SongId);