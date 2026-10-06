using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.GetPublicPlaylists;

public sealed class GetPublicPlaylistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetPublicPlaylistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PlaylistEntity>> HandleAsync(
        GetPublicPlaylistsQuery query,
        CancellationToken cancellationToken = default)
    {
        var limit = query.Limit <= 0
            ? 20
            : Math.Min(query.Limit, 100);

        return await _dbContext.Playlists
            .AsNoTracking()
            .Where(playlist => playlist.IsPublic)
            .OrderByDescending(playlist => playlist.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}