using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Downloads.DownloadSong;

public sealed class DownloadSongHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DownloadSongHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<DownloadHistory> HandleAsync(
        DownloadSongCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        if (command.SongId == Guid.Empty)
        {
            throw new ArgumentException(
                "Song ID is required.",
                nameof(command.SongId));
        }

        var userExists = await _dbContext.Users
            .AnyAsync(
                user =>
                    user.Id == command.UserId &&
                    user.IsActive,
                cancellationToken);

        if (!userExists)
        {
            throw new KeyNotFoundException(
                "Active user was not found.");
        }

        var song = await _dbContext.Songs
            .FirstOrDefaultAsync(
                song =>
                    song.Id == command.SongId &&
                    song.IsPublished,
                cancellationToken);

        if (song is null)
        {
            throw new KeyNotFoundException(
                "Published song was not found.");
        }

        var downloadHistory = new DownloadHistory(
            command.UserId,
            command.SongId);

        _dbContext.DownloadHistories.Add(downloadHistory);

        await _dbContext.SaveChangesAsync(cancellationToken);

        downloadHistory = await _dbContext.DownloadHistories
            .Include(history => history.Song)
            .FirstAsync(
                history => history.Id == downloadHistory.Id,
                cancellationToken);

        return downloadHistory;
    }
}