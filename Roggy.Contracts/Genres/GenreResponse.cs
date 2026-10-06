namespace Roggy.Contracts.Genres;

public sealed record GenreResponse(
    Guid Id,
    string Name,
    string Slug,
    DateTime CreatedAt,
    DateTime UpdatedAt);