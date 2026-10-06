using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.Downloads.GetDownloadHistory;

public sealed class GetDownloadHistoryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetDownloadHistoryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Roggy.Domain.Entities.DownloadHistory>>
        HandleAsync(
            GetDownloadHistoryQuery query,
            CancellationToken cancellationToken = default)
    {
        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        var limit = query.Limit <= 0
            ? 50
            : Math.Min(query.Limit, 100);

        return await _dbContext.DownloadHistories
            .AsNoTracking()
            .Include(history => history.Song)
            .Where(history => history.UserId == query.UserId)
            .OrderByDescending(history => history.DownloadedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}