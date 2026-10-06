using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.SearchSongs;

public sealed class SearchSongsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public SearchSongsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Song>> HandleAsync(
        SearchSongsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query.Query))
        {
            return [];
        }

        var searchTerm = query.Query.Trim().ToLower();

        return await _dbContext.Songs
            .AsNoTracking()
            .Where(song =>
                song.IsPublished &&
                (
                    song.Title.ToLower().Contains(searchTerm) ||
                    song.Artist.StageName
                        .ToLower()
                        .Contains(searchTerm) ||
                    song.Genre.Name
                        .ToLower()
                        .Contains(searchTerm)
                ))
            .OrderBy(song => song.Title)
            .ToListAsync(cancellationToken);
    }
}