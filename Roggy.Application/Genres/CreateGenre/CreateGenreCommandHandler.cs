using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Genres.CreateGenre;

public sealed class CreateGenreHandler
{
    private readonly IApplicationDbContext _dbContext;

    public CreateGenreHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Genre> HandleAsync(
        CreateGenreCommand command,
        CancellationToken cancellationToken = default)
    {
        var name = command.Name.Trim();
        var slug = command.Slug.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Genre name is required.",
                nameof(command.Name));
        }

        if (string.IsNullOrWhiteSpace(slug))
        {
            throw new ArgumentException(
                "Genre slug is required.",
                nameof(command.Slug));
        }

        var nameExists = await _dbContext.Genres
            .AnyAsync(
                genre => genre.Name == name,
                cancellationToken);

        if (nameExists)
        {
            throw new InvalidOperationException(
                "A genre with this name already exists.");
        }

        var slugExists = await _dbContext.Genres
            .AnyAsync(
                genre => genre.Slug == slug,
                cancellationToken);

        if (slugExists)
        {
            throw new InvalidOperationException(
                "A genre with this slug already exists.");
        }

        var genre = new Genre(
            name,
            slug);

        _dbContext.Genres.Add(genre);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return genre;
    }
}