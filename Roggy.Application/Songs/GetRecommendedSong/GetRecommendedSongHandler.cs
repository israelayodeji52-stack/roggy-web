using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.GetRecommendedSongs;

public sealed class GetRecommendedSongsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetRecommendedSongsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Song>> HandleAsync(
        GetRecommendedSongsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit <= 0
            ? 20
            : Math.Min(query.Limit, 100);

        var preferredGenreIds = await _dbContext.ListeningHistories
            .AsNoTracking()
            .Where(history => history.UserId == query.UserId)
            .Join(
                _dbContext.Songs,
                history => history.SongId,
                song => song.Id,
                (history, song) => song.GenreId)
            .GroupBy(genreId => genreId)
            .OrderByDescending(group => group.Count())
            .Select(group => group.Key)
            .Take(5)
            .ToListAsync(cancellationToken);

        // No listening history yet.
        // Fall back to trending songs.
        if (preferredGenreIds.Count == 0)
        {
            return await _dbContext.Songs
                .AsNoTracking()
                .Where(song => song.IsPublished)
                .OrderByDescending(song => song.PlayCount)
                .ThenByDescending(song => song.ReleaseDate)
                .ThenByDescending(song => song.CreatedAt)
                .Take(limit)
                .ToListAsync(cancellationToken);
        }

        var recommendedSongs = await _dbContext.Songs
            .AsNoTracking()
            .Where(song =>
                song.IsPublished &&
                preferredGenreIds.Contains(song.GenreId))
            .OrderByDescending(song => song.PlayCount)
            .ThenByDescending(song => song.ReleaseDate)
            .ThenByDescending(song => song.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);

        if (recommendedSongs.Count >= limit)
        {
            return recommendedSongs;
        }

        var existingSongIds = recommendedSongs
            .Select(song => song.Id)
            .ToList();

        var remainingLimit = limit - recommendedSongs.Count;

        var trendingSongs = await _dbContext.Songs
            .AsNoTracking()
            .Where(song =>
                song.IsPublished &&
                !existingSongIds.Contains(song.Id))
            .OrderByDescending(song => song.PlayCount)
            .ThenByDescending(song => song.ReleaseDate)
            .ThenByDescending(song => song.CreatedAt)
            .Take(remainingLimit)
            .ToListAsync(cancellationToken);

        return recommendedSongs
            .Concat(trendingSongs)
            .ToList();
    }
}