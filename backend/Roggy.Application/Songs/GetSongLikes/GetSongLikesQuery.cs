namespace Roggy.Application.Songs.GetSongLikes;

public sealed record GetSongLikesQuery(
    Guid SongId,
    Guid? UserId);