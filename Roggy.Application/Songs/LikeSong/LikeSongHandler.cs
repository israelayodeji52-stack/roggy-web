using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.LikeSong;

public sealed class LikeSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public LikeSongHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SongLike> HandleAsync(
        LikeSongCommand command,
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

        var existingLike = await _dbContext.SongLikes
            .FirstOrDefaultAsync(
                like => like.SongId == command.SongId &&
                        like.UserId == command.UserId,
                cancellationToken);

        if (existingLike is not null)
        {
            throw new InvalidOperationException(
                "You have already liked this song.");
        }

        var like = new SongLike(
            command.UserId,
            command.SongId);

        _dbContext.SongLikes.Add(like);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return like;
    }
}