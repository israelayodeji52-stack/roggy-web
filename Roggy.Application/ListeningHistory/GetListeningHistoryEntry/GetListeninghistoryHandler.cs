using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;

namespace Roggy.Application.ListeningHistory.GetListeningHistoryEntry;

public sealed class GetListeningHistoryEntryHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetListeningHistoryEntryHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Roggy.Domain.Entities.ListeningHistory?> HandleAsync(
        GetListeningHistoryEntryQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.HistoryId == Guid.Empty)
        {
            throw new ArgumentException(
                "History ID is required.",
                nameof(query.HistoryId));
        }

        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        return await _dbContext.ListeningHistories
            .AsNoTracking()
            .Include(history => history.Song)
            .FirstOrDefaultAsync(
                history =>
                    history.Id == query.HistoryId &&
                    history.UserId == query.UserId,
                cancellationToken);
    }
}