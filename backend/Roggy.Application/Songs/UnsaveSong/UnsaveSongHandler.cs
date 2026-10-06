using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Songs.UnsaveSong;

public sealed class UnsaveSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnsaveSongHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        UnsaveSongCommand command,
        CancellationToken cancellationToken = default)
    {
        var savedSong = await _dbContext.SavedSongs
            .FirstOrDefaultAsync(
                savedSong =>
                    savedSong.SongId == command.SongId &&
                    savedSong.UserId == command.UserId,
                cancellationToken);

        if (savedSong is null)
        {
            throw new KeyNotFoundException(
                "You have not saved this song.");
        }

        _dbContext.SavedSongs.Remove(savedSong);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}