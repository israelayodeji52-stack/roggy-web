using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.GetUserPlaylists;

public sealed class GetUserPlaylistsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetUserPlaylistsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<PlaylistEntity>> HandleAsync(
        GetUserPlaylistsQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Playlists
            .AsNoTracking()
            .Where(playlist => playlist.UserId == query.UserId)
            .OrderByDescending(playlist => playlist.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}