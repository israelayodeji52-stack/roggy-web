using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Songs.GetSongLikes;

public sealed class GetSongLikesHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetSongLikesHandler(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<(int LikeCount, bool IsLiked)> HandleAsync(
        GetSongLikesQuery query,
        CancellationToken cancellationToken = default)
    {
        var songExists = await _dbContext.Songs
            .AnyAsync(
                song => song.Id == query.SongId,
                cancellationToken);

        if (!songExists)
        {
            throw new KeyNotFoundException("Song was not found.");
        }

        var likeCount = await _dbContext.SongLikes
            .CountAsync(
                like => like.SongId == query.SongId,
                cancellationToken);

        var isLiked = false;

        if (query.UserId.HasValue)
        {
            isLiked = await _dbContext.SongLikes
                .AnyAsync(
                    like => like.SongId == query.SongId &&
                            like.UserId == query.UserId.Value,
                    cancellationToken);
        }

        return (likeCount, isLiked);
    }
}