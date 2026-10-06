namespace Roggy.Application.Songs.GetSavedSongStatus;

public sealed record GetSavedSongStatusQuery(
    Guid SongId,
    Guid UserId);