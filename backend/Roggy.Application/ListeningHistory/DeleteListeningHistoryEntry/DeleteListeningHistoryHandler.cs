using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.ListeningHistory.DeleteListeningHistoryEntry;

public sealed class DeleteListeningHistoryEntryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteListeningHistoryEntryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeleteListeningHistoryEntryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.HistoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "History ID is required.",
                nameof(command.HistoryId));
        }

        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        var history = await _dbContext.ListeningHistories
            .FirstOrDefaultAsync(
                entry =>
                    entry.Id == command.HistoryId &&
                    entry.UserId == command.UserId,
                cancellationToken);

        if (history is null)
        {
            throw new KeyNotFoundException(
                "Listening history entry was not found.");
        }

        _dbContext.ListeningHistories.Remove(history);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}