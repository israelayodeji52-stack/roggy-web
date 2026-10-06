namespace Roggy.Application.Genres.CreateGenre;

public sealed record CreateGenreCommand(
    string Name,
    string Slug);