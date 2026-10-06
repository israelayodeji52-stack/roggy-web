using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.ListeningHistory.GetListeningHistory;

public sealed class GetListeningHistoryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetListeningHistoryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Roggy.Domain.Entities.ListeningHistory>> HandleAsync(
        GetListeningHistoryQuery query,
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

        return await _dbContext.ListeningHistories
            .AsNoTracking()
            .Include(history => history.Song)
            .Where(history => history.UserId == query.UserId)
            .OrderByDescending(history => history.StartedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}