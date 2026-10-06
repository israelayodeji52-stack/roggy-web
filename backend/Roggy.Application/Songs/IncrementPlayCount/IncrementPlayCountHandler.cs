using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.IncrementPlayCount;

public sealed class IncrementPlayCountHandler
{
    private readonly IApplicationDbContext _dbContext;

    public IncrementPlayCountHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song> HandleAsync(
        IncrementPlayCountCommand command,
        CancellationToken cancellationToken = default)
    {
        var song = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song => song.Id == command.SongId,
                cancellationToken);

        if (song is null)
        {
            throw new KeyNotFoundException("Song was not found.");
        }

        song.IncrementPlayCount();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return song;
    }
}