using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using PlaylistEntity = Roggy.Domain.Entities.Playlist;

namespace Roggy.Application.Playlist.DeletePlaylist;

public sealed class DeletePlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeletePlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeletePlaylistCommand command,
        CancellationToken cancellationToken = default)
    {
        var playlist = await _dbContext.Playlists
            .FirstOrDefaultAsync(
                playlist =>
                    playlist.Id == command.PlaylistId &&
                    playlist.UserId == command.UserId,
                cancellationToken);

        if (playlist is null)
        {
            throw new KeyNotFoundException(
                "Playlist was not found.");
        }

        _dbContext.Playlists.Remove(playlist);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}