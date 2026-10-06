using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.PublishSong;

public sealed class PublishSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public PublishSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song> HandleAsync(
        PublishSongCommand command,
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

        song.Publish();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return song;
    }
}