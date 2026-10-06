namespace Roggy.Application.Artists.GetPopularArtists;

public sealed record GetPopularArtistsQuery(
    int Limit = 20);