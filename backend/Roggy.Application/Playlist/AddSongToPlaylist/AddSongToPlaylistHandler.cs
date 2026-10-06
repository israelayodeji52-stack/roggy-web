using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Playlist.AddSongToPlaylist;

public sealed class AddSongToPlaylistHandler
{
    private readonly IApplicationDbContext _dbContext;

    public AddSongToPlaylistHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PlaylistItem> HandleAsync(
        AddSongToPlaylistCommand command,
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

        var songExists = await _dbContext.Songs
            .AnyAsync(
                song =>
                    song.Id == command.SongId &&
                    song.IsPublished,
                cancellationToken);

        if (!songExists)
        {
            throw new KeyNotFoundException(
                "Published song was not found.");
        }

        var alreadyExists = await _dbContext.PlaylistItems
            .AnyAsync(
                item =>
                    item.PlaylistId == command.PlaylistId &&
                    item.SongId == command.SongId,
                cancellationToken);

        if (alreadyExists)
        {
            throw new InvalidOperationException(
                "Song is already in this playlist.");
        }

        var lastPosition = await _dbContext.PlaylistItems
            .Where(item =>
                item.PlaylistId == command.PlaylistId)
            .Select(item => (int?)item.Position)
            .MaxAsync(cancellationToken) ?? 0;

        var playlistItem = new PlaylistItem(
            command.PlaylistId,
            command.SongId,
            lastPosition + 1);

        _dbContext.PlaylistItems.Add(playlistItem);

        await _dbContext.SaveChangesAsync(cancellationToken);

        return playlistItem;
    }
}