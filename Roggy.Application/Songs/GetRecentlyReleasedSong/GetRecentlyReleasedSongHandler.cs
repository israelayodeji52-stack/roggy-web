using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.GetRecentlyReleasedSongs;

public sealed class GetRecentlyReleasedSongsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetRecentlyReleasedSongsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Song>> HandleAsync(
        GetRecentlyReleasedSongsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit <= 0
            ? 20
            : Math.Min(query.Limit, 100);

        return await _dbContext.Songs
            .AsNoTracking()
            .Where(song =>
                song.IsPublished &&
                song.ReleaseDate != null)
            .OrderByDescending(song => song.ReleaseDate)
            .ThenByDescending(song => song.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}