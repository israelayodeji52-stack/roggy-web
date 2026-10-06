using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Playlist.ReorderPlaylistSong;

public sealed class ReorderPlaylistSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public ReorderPlaylistSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        ReorderPlaylistSongCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.NewPosition <= 0)
        {
            throw new ArgumentException(
                "New position must be greater than zero.",
                nameof(command.NewPosition));
        }

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

        var items = await _dbContext.PlaylistItems
            .Where(item =>
                item.PlaylistId == command.PlaylistId)
            .OrderBy(item => item.Position)
            .ToListAsync(cancellationToken);

        var item = items.FirstOrDefault(
            playlistItem => playlistItem.SongId == command.SongId);

        if (item is null)
        {
            throw new KeyNotFoundException(
                "Song was not found in this playlist.");
        }

        var maxPosition = items.Count;

        if (command.NewPosition > maxPosition)
        {
            throw new ArgumentException(
                $"New position cannot be greater than {maxPosition}.",
                nameof(command.NewPosition));
        }

        var oldPosition = item.Position;

        if (oldPosition == command.NewPosition)
        {
            return;
        }

        /*
         * Move every item to a unique temporary position first.
         *
         * Example:
         *   1, 2
         * becomes:
         *   3, 4
         *
         * This avoids violating the unique
         * (PlaylistId, Position) index while reordering.
         */
        for (var index = 0; index < items.Count; index++)
        {
            items[index].ChangePosition(
                maxPosition + index + 1);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        /*
         * Build the new order in memory.
         */
        var reorderedItems = items
            .OrderBy(playlistItem =>
                playlistItem.Id == item.Id
                    ? command.NewPosition
                    : playlistItem.Position)
            .ToList();

        reorderedItems.Remove(item);

        reorderedItems.Insert(
            command.NewPosition - 1,
            item);

        /*
         * Assign the final positions.
         */
        for (var index = 0; index < reorderedItems.Count; index++)
        {
            reorderedItems[index].ChangePosition(index + 1);
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}