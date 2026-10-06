using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Notifications.GetNotifications;

public sealed class GetNotificationsHandler
{
    private readonly IApplicationDbContext _dbContext;

    public GetNotificationsHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Notification>> HandleAsync(
        GetNotificationsQuery query,
        CancellationToken cancellationToken = default)
    {
        if (query.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(query.UserId));
        }

        if (query.Limit <= 0)
        {
            throw new ArgumentException(
                "Limit must be greater than zero.",
                nameof(query.Limit));
        }

        var limit = Math.Min(query.Limit, 100);

        return await _dbContext.Notifications
            .Where(notification => notification.UserId == query.UserId)
            .OrderByDescending(notification => notification.CreatedAt)
            .Take(limit)
            .ToListAsync(cancellationToken);
    }
}