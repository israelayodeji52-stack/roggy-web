using Microsoft.EntityFrameworkCore;
using Roggy.Application.Abstractions.Persistence;
using Roggy.Domain.Entities;

namespace Roggy.Application.Notifications.MarkNotificationAsRead;

public sealed class MarkNotificationAsReadHandler
{
    private readonly IApplicationDbContext _dbContext;

    public MarkNotificationAsReadHandler(
        IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Notification> HandleAsync(
        MarkNotificationAsReadCommand command,
        CancellationToken cancellationToken = default)
    {
        if (command.UserId == Guid.Empty)
        {
            throw new ArgumentException(
                "User ID is required.",
                nameof(command.UserId));
        }

        if (command.NotificationId == Guid.Empty)
        {
            throw new ArgumentException(
                "Notification ID is required.",
                nameof(command.NotificationId));
        }

        var notification = await _dbContext.Notifications
            .FirstOrDefaultAsync(
                item =>
                    item.Id == command.NotificationId &&
                    item.UserId == command.UserId,
                cancellationToken);

        if (notification is null)
        {
            throw new KeyNotFoundException(
                "Notification was not found.");
        }

        notification.MarkAsRead();

        await _dbContext.SaveChangesAsync(cancellationToken);

        return notification;
    }
}