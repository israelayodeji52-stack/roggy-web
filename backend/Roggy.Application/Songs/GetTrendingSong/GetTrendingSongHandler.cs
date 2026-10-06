using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.GetTrendingSongs;

public sealed class GetTrendingSongsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetTrendingSongsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Song>> HandleAsync(
        GetTrendingSongsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit <= 0
            ? 20
            : Math.Min(query.Limit, 100);

        return await _dbContext.Songs
            .AsNoTracking()
            .Where(song => song.IsPublished)
            .OrderByDescending(song => song.PlayCount)
            .ThenByDescending(song => song.ReleaseDate)
            .ThenByDescending(song => song.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}