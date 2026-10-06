using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Songs.DeleteSong;

public sealed class DeleteSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeleteSongCommand command,
        CancellationToken cancellationToken = default)
    {
        var song = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song => song.Id == command.SongId,
                cancellationToken);

        if (song is null)
        {
            throw new KeyNotFoundException(
                "Song was not found.");
        }

        _dbContext.Songs.Remove(song);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}