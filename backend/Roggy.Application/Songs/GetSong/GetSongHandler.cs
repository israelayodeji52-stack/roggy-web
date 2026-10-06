using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.GetSong;

public sealed class GetSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Song> HandleAsync(
        GetSongQuery query,
        CancellationToken cancellationToken = default)
    {
        var song = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song => song.Id == query.SongId,
                cancellationToken);

        if (song is null)
        {
            throw new KeyNotFoundException(
                "Song was not found.");
        }

        return song;
    }
}