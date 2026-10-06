using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Songs.GetLikedSongs;

public sealed class GetLikedSongsHandler
{
    private readonly IApplicationDbContext _context;

    public GetLikedSongsHandler(
        IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Song>> HandleAsync(
        GetLikedSongsQuery query,
        CancellationToken cancellationToken)
    {
        return await _context.SongLikes
            .Where(songLike => songLike.UserId == query.UserId)
            .OrderByDescending(songLike => songLike.CreatedAt)
            .Select(songLike => songLike.Song)
            .Where(song => song.IsPublished)
            .ToListAsync(cancellationToken);
    }
}