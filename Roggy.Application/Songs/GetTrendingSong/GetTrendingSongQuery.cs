namespace Roggy.Application.Songs.GetTrendingSongs;

public sealed record GetTrendingSongsQuery(
    int Limit = 20);