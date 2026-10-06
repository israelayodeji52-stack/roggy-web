using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.UnpublishSong;

public sealed class UnpublishSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnpublishSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song> HandleAsync(
        UnpublishSongCommand command,
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

        song.Unpublish();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return song;
    }
}