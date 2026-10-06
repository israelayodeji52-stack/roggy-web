using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Downloads.DeleteDownloadHistoryEntry;

public sealed class DeleteDownloadHistoryEntryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public DeleteDownloadHistoryEntryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task HandleAsync(
        DeleteDownloadHistoryEntryCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.DownloadHistoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "Download history ID is required.",
                nameof(command.DownloadHistoryId));
        }

        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        var downloadHistory =
            await _dbContext.DownloadHistories
                .FirstOrDefaultAsync(
                    history =>
                        history.Id == command.DownloadHistoryId &&
                        history.UserId == command.UserId,
                    cancellationToken);

        if (downloadHistory is null)
        {
            throw new KeyNotFoundException(
                "Download history entry was not found.");
        }

        _dbContext.DownloadHistories.Remove(downloadHistory);

        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}