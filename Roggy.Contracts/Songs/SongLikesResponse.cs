namespace Roggy.Contracts.Songs;

public sealed record SongLikesResponse(
    Guid SongId,
    int LikeCount,
    bool IsLiked);