using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using ListeningHistoryEntity = Roggy.Domain.Entities.ListeningHistory;

namespace Roggy.Application.ListeningHistory.RecordListeningHistory;

public sealed class RecordListeningHistoryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public RecordListeningHistoryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ListeningHistoryEntity> HandleAsync(
        RecordListeningHistoryCommand command,
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

        if (command.DurationPlayed < TimeSpan.Zero)
        {
            throw new ArgumentException(
                "Duration played cannot be negative.",
                nameof(command.DurationPlayed));
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

        var startedAt = command.StartedAt == default
            ? DateTime.UtcNow
            : command.StartedAt;

        var listeningHistory = new ListeningHistoryEntity(
            command.UserId,
            command.SongId,
            startedAt);

        if (command.CompletedAt.HasValue)
        {
            listeningHistory.Complete(
                command.CompletedAt.Value,
                command.DurationPlayed);
        }
        else if (command.DurationPlayed > TimeSpan.Zero)
        {
            listeningHistory.Complete(
                startedAt.Add(command.DurationPlayed),
                command.DurationPlayed);
        }

        _dbContext.ListeningHistories.Add(listeningHistory);

        await _dbContext.SaveChangesAsync(cancellationToken);

        /*
         * Load the song navigation property so the API
         * response can include the song title and cover image.
         */
        listeningHistory = await _dbContext.ListeningHistories
            .Include(history => history.Song)
            .FirstAsync(
                history => history.Id == listeningHistory.Id,
                cancellationToken);

        return listeningHistory;
    }
}