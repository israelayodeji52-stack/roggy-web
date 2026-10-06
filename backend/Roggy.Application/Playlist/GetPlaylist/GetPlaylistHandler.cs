using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.GetPlaylist;

public sealed class GetPlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetPlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistEntity?> HandleAsync(
        GetPlaylistQuery query,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Playlists
            .AsNoTracking()
            .Include(playlist => playlist.Items)
                .ThenInclude(item => item.Song)
            .FirstOrDefaultAsync(
                playlist =>
                    playlist.Id == query.PlaylistId &&
                    (
                        playlist.UserId == query.UserId ||
                        playlist.IsPublic
                    ),
                cancellationToken);
    }
}