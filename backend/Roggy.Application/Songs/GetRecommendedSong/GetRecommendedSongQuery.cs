namespace Roggy.Application.Songs.GetRecommendedSongs;

public sealed record GetRecommendedSongsQuery(
    Guid UserId,
    int Limit = 20);