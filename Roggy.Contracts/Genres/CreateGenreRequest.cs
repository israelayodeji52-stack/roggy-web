namespace Roggy.Contracts.Genres;

public sealed record CreateGenreRequest(
    string Name,
    string Slug);