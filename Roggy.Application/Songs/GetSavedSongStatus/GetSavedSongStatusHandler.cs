using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Songs.GetSavedSongStatus;

public sealed class GetSavedSongStatusHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetSavedSongStatusHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> HandleAsync(
        GetSavedSongStatusQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.SavedSongs
            .AnyAsync(
                savedSong =>
                    savedSong.SongId == query.SongId &&
                    savedSong.UserId == query.UserId,
                cancellationToken);
    }
}