using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.UnlikeSong;

public sealed class UnlikeSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public UnlikeSongHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SongLike> HandleAsync(
        UnlikeSongCommand command,
        CancellationToken cancellationToken = default)
    {
        var like = await _dbContext.SongLikes
            .FirstOrDefaultAsync(
                like => like.SongId == command.SongId &&
                        like.UserId == command.UserId,
                cancellationToken);

        if (like is null)
        {
            throw new KeyNotFoundException(
                "You have not liked this song.");
        }

        _dbContext.SongLikes.Remove(like);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return like;
    }
}