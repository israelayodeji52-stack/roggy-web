using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Songs.GetSavedSongs;

public sealed class GetSavedSongsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetSavedSongsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Roggy.Domain.Entities.Song>> HandleAsync(
        GetSavedSongsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SavedSongs
            .Where(savedSong => savedSong.UserId == query.UserId)
            .Include(savedSong => savedSong.Song)
            .ThenInclude(song => song.Artist)
            .Include(savedSong => savedSong.Song)
            .ThenInclude(song => song.Genre)
            .Select(savedSong => savedSong.Song)
            .OrderByDescending(song => song.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}