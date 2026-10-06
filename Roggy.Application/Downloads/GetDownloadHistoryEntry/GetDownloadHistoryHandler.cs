using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Downloads.GetDownloadHistoryEntry;

public sealed class GetDownloadHistoryEntryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetDownloadHistoryEntryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.DownloadHistory?> HandleAsync(
        GetDownloadHistoryEntryQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.DownloadHistoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "Download history ID is required.",
                nameof(query.DownloadHistoryId));
        }

        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        return await _dbContext.DownloadHistories
            .AsNoTracking()
            .Include(history => history.Song)
            .FirstOrDefaultAsync(
                history =>
                    history.Id == query.DownloadHistoryId &&
                    history.UserId == query.UserId,
                cancellationToken);
    }
}