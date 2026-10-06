using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Application.Songs.SaveSong;

namespace Roggy.Application.Songs.SavedSong;

public sealed class SavedSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public SavedSongHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.SavedSong> HandleAsync(
        SaveSongCommand command,
        CancellationToken cancellationToken = default)
    {
        var songExists = await _dbContext.Songs
            .AnyAsync(
                song => song.Id == command.SongId,
                cancellationToken);

        if (!songExists)
        {
            throw new KeyNotFoundException("Song was not found.");
        }

        var existingSavedSong = await _dbContext.SavedSongs
            .FirstOrDefaultAsync(
                savedSong =>
                    savedSong.SongId == command.SongId &&
                    savedSong.UserId == command.UserId,
                cancellationToken);

        if (existingSavedSong is not null)
        {
            throw new InvalidOperationException(
                "You have already saved this song.");
        }

        var savedSong = new Roggy.Domain.Entities.SavedSong(
            command.UserId,
            command.SongId);

        _dbContext.SavedSongs.Add(savedSong);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return savedSong;
    }
}