using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Playlist.RemoveSongFromPlaylist;

public sealed class RemoveSongFromPlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public RemoveSongFromPlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        RemoveSongFromPlaylistCommand command,
        CancellationToken cancellationToken = default)
    {
        var playlistExists = await _dbContext.Playlists
            .AnyAsync(
                playlist =>
                    playlist.Id == command.PlaylistId &&
                    playlist.UserId == command.UserId,
                cancellationToken);

        if (!playlistExists)
        {
            throw new KeyNotFoundException(
                "Playlist was not found.");
        }

        var playlistItem = await _dbContext.PlaylistItems
            .FirstOrDefaultAsync(
                item =>
                    item.PlaylistId == command.PlaylistId &&
                    item.SongId == command.SongId,
                cancellationToken);

        if (playlistItem is null)
        {
            throw new KeyNotFoundException(
                "Song was not found in this playlist.");
        }

        _dbContext.PlaylistItems.Remove(playlistItem);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}