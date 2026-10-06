namespace Roggy.Application.Songs.GetRecentlyReleasedSongs;

public sealed record GetRecentlyReleasedSongsQuery(
    int Limit = 20);